using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleServiceCollectionSdk.Http.Extensions;

/// <summary>
/// Extension methods for MultipartFormDataContent to serialize objects into multipart/form-data format.
/// Handles primitives, byte arrays (files), file arrays, nested objects, and Optional wrapper types.
/// </summary>
public static class MultipartFormDataContentExtensions
{
    /// <summary>
    /// Recursively serializes an object into multipart/form-data format.
    /// Maps object properties to form fields, handling files (byte[]), file arrays,
    /// nested objects, and primitives. Respects JSON property name attributes and
    /// Optional wrapper semantics.
    /// </summary>
    /// <param name="formData">The multipart form data content to populate.</param>
    /// <param name="content">The object to serialize.</param>
    /// <param name="options">JSON serializer options for property name mapping.</param>
    /// <returns>The populated multipart form data content for method chaining.</returns>
    public static MultipartFormDataContent AddObject(
        this MultipartFormDataContent formData,
        object content,
        JsonSerializerOptions? options
    )
    {
        foreach (var property in content.GetType().GetProperties())
        {
            // Skip the [JsonExtensionData] overflow bag: it holds unknown JSON
            // properties and has no meaning as multipart form fields. Without this
            // the dictionary itself would be walked and its CLR members (Count,
            // Keys, ...) emitted as bogus fields.
            if (property.GetCustomAttribute<JsonExtensionDataAttribute>() != null)
            {
                continue;
            }

            var value = property.GetValue(content);
            var mappedKey = GetPropertyName(property);

            if (IsOptionalType(property.PropertyType))
            {
                // An unset Optional is omitted entirely rather than sent as an empty field.
                value = GetOptionalValue(value);
                if (value == null)
                {
                    continue;
                }
            }

            formData.AddValue(mappedKey, value, options);
        }

        return formData;
    }

    private static void AddValue(
        this MultipartFormDataContent formData,
        string mappedKey,
        object? value,
        JsonSerializerOptions? options
    )
    {
        if (value is byte[] fileBytes)
        {
            formData.AddFilePart(mappedKey, fileBytes);
        }
        else if (value is IEnumerable<byte[]> files)
        {
            foreach (var file in files)
            {
                formData.AddFilePart(mappedKey, file);
            }
        }
        else if (
            value is System.Collections.IEnumerable items
            && value is not string
            && IsPrimitiveEnumerable(value)
        )
        {
            // An array of primitives (e.g. List<string>) is sent as one repeated field
            // per element rather than reflected into the List's CLR members (Count, ...).
            // Object/dictionary collections fall through to the nested-object branch.
            foreach (var item in items)
            {
                formData.Add(new StringContent(item?.ToString() ?? string.Empty), mappedKey);
            }
        }
        else if (value != null && !IsPrimitive(value.GetType()))
        {
            var nestedContent = new MultipartFormDataContent().AddObject(value, options);
            formData.Add(nestedContent, mappedKey);
        }
        else
        {
            formData.Add(new StringContent(value?.ToString() ?? string.Empty), mappedKey);
        }
    }

    private static void AddFilePart(
        this MultipartFormDataContent formData,
        string mappedKey,
        byte[] fileBytes
    )
    {
        var fileContent = new StreamContent(new MemoryStream(fileBytes));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        // Quote name/filename explicitly (RFC 7578). .NET's default is unquoted
        // (name=x; filename=x), which stricter multipart parsers reject.
        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "\"" + mappedKey + "\"",
            FileName = "\"" + mappedKey + "\"",
        };

        formData.Add(fileContent);
    }

    private static bool IsPrimitive(Type type)
    {
        return type.IsPrimitive || type.IsValueType || type == typeof(string);
    }

    private static bool IsPrimitiveEnumerable(object value)
    {
        var elementType = GetEnumerableElementType(value.GetType());
        // KeyValuePair<,> is a struct, so IsPrimitive would otherwise treat a dictionary's
        // entries as primitives; exclude it so dictionaries fall through to the
        // nested-object branch instead of stringifying each "[key, value]".
        return elementType != null
            && IsPrimitive(elementType)
            && !(
                elementType.IsGenericType
                && elementType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)
            );
    }

    private static Type? GetEnumerableElementType(Type type)
    {
        if (type.IsArray)
        {
            return type.GetElementType();
        }

        foreach (var iface in type.GetInterfaces())
        {
            if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return iface.GetGenericArguments()[0];
            }
        }

        return null;
    }

    private static bool IsOptionalType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition().Name == "Optional`1";
    }

    private static object? GetOptionalValue(object? optional)
    {
        if (optional == null)
        {
            return null;
        }

        var hasValueProperty = optional.GetType().GetProperty("IsProvided");
        if (hasValueProperty?.GetValue(optional) is true)
        {
            var valueProperty = optional.GetType().GetProperty("Value");
            return valueProperty?.GetValue(optional);
        }
        return null;
    }

    private static string GetPropertyName(PropertyInfo property)
    {
        var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
        if (jsonPropertyAttribute != null)
        {
            return jsonPropertyAttribute.Name;
        }
        return property.Name;
    }
}
