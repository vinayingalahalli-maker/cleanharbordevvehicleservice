using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;

namespace VehicleServiceCollectionSdk.Http.Serialization;

// Wire formats for date primitives in path/query/header strings; kept in sync
// with the JSON DateTimeSerializer / DateOnlyConverter.

public static class Serializer
{
    /// <summary>
    /// Serializes types into strings based on the RFC-6570 URI Template and OpenAPI specifications.
    /// <see cref="https://datatracker.ietf.org/doc/html/rfc6570"/>
    /// <see cref="https://swagger.io/docs/specification/serialization/"/>
    /// </summary>
    public static string Serialize<T>(
        string key,
        T value,
        SerializationStyle style,
        bool explode = true,
        bool shouldUrlEncode = true
    )
    {
        return value switch
        {
            null
            or string
            or bool
            or int
            or long
            or double
            or uint
            or ulong
            or DateTime
            or DateOnly => SerializePrimitive(key, value, style, shouldUrlEncode),
            IEnumerable e => SerializeEnumerable(key, e, style, explode, shouldUrlEncode),
            object o => SerializeObject(key, o, style, explode, shouldUrlEncode),
        };
    }

    private static string SerializeValue(object? value, bool shouldUrlEncode = true)
    {
        return value switch
            {
                null => "null",
                string s => shouldUrlEncode ? Uri.EscapeDataString(s) : s,
                bool b => b.ToString().ToLowerInvariant(),
                int or long or double or uint or ulong => value.ToString(),
                DateTime dt => dt.ToString(
                    "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK",
                    CultureInfo.InvariantCulture
                ),
                DateOnly d => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                IEnumerable e => SerializeEnumerable(
                    string.Empty,
                    e,
                    SerializationStyle.Simple,
                    false
                ),
                not null => SerializeObject(string.Empty, value, SerializationStyle.Simple, false),
            } ?? string.Empty;
    }

    private static string SerializePrimitive(
        string key,
        object? value,
        SerializationStyle style,
        bool shouldUrlEncode = true
    )
    {
        return style switch
        {
            SerializationStyle.Label => $".{SerializeValue(value, shouldUrlEncode)}",
            // Matrix ("path-style") expansion is `;name=value` (RFC-6570 §3.2.7). Dropping the
            // `{key}=` half produced `;42` instead of `;matrix=42`, so the server saw an unnamed
            // segment. Kept in sync with the enumerable/object Matrix branches below.
            SerializationStyle.Matrix => $";{key}={SerializeValue(value, shouldUrlEncode)}",
            SerializationStyle.Form => $"{key}={SerializeValue(value, shouldUrlEncode)}",
            _ => SerializeValue(value, shouldUrlEncode),
        };
    }

    private static string SerializeEnumerable(
        string key,
        IEnumerable enumerable,
        SerializationStyle style,
        bool explode,
        bool shouldUrlEncode = true
    )
    {
        var array = enumerable as object[] ?? enumerable.Cast<object>().ToArray();
        var serializedValues = array.Select((object o) => SerializeValue(o, shouldUrlEncode));

        switch (style)
        {
            case SerializationStyle.Simple:
                return string.Join(",", serializedValues);
            case SerializationStyle.Label:
                return explode
                    ? string.Join(".", serializedValues)
                    : $".{string.Join(",", serializedValues)}";
            case SerializationStyle.Matrix:
                return explode
                    ? string.Join($";{key}=", serializedValues)
                    : $";{key}={string.Join(",", serializedValues)}";
        }

        // An EMPTY collection is an absent parameter, not a present-but-empty one. Every
        // non-exploded style below used to fall through to `$"{key}={...}"` and emit `key=`, which a
        // server reads as a single empty element (`?formJoined=&spaceDelimited=&pipeDelimited=`).
        // Callers (SetQueryParameter/SetHeader/SetCookieParameter) skip an empty string, so
        // returning one here omits the parameter entirely — matching the already-correct
        // explode: true behaviour.
        //
        // Deliberately placed AFTER the switch above: Simple/Label/Matrix are the PATH styles, and
        // SetPathParameter substitutes into the URL template without skipping an empty string, so
        // short-circuiting them would change the URL shape (`/p/.` -> `/p/`, `/p/;key=` -> `/p/`)
        // rather than omit anything. Path styles keep their previous rendering.
        if (array.Length == 0)
        {
            return string.Empty;
        }

        if (explode)
            return string.Join(
                $"&",
                array.Select(e => Serialize(key, e, SerializationStyle.Form, explode))
            );

        var separator = style switch
        {
            SerializationStyle.SpaceDelimited => " ",
            SerializationStyle.PipeDelimited => "|",
            _ => ",",
        };

        return $"{key}={string.Join(separator, serializedValues)}";
    }

    /// <summary>
    /// Whether a property is unconditionally excluded from the wire by [JsonIgnore].
    /// </summary>
    /// <remarks>
    /// The condition matters. A bare [JsonIgnore] means "never serialize" — that is how the
    /// public AdditionalProperties bag is declared, and reflecting over it emitted an undeclared
    /// `AdditionalProperties=` pair on every request carrying an object parameter. But
    /// [JsonIgnore(Condition = WhenWritingDefault)] is what every optional property carries, and
    /// those ARE wire fields, so a blanket check on the attribute would drop them all.
    /// </remarks>
    private static bool IsIgnoredAlways(PropertyInfo property)
    {
        var ignore =
            property.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).FirstOrDefault()
            as JsonIgnoreAttribute;

        return ignore != null && ignore.Condition == JsonIgnoreCondition.Always;
    }

    private static string SerializeObject<T>(
        string key,
        T o,
        SerializationStyle style,
        bool explode,
        bool shouldUrlEncode = true
    )
        where T : class
    {
        var properties = o.GetType()
            .GetProperties()
            // Skip the [JsonExtensionData] bag so unknown fields never leak into query/path strings.
            .Where(p => !p.IsDefined(typeof(JsonExtensionDataAttribute), false))
            .Where(p => !IsIgnoredAlways(p))
            .Select(p =>
            {
                // Use JsonPropertyNameAttribute for property name if available
                var name = p.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false)
                    .FirstOrDefault()
                    is JsonPropertyNameAttribute jsonProperty
                    ? jsonProperty.Name
                    : p.Name;
                var value = p.GetValue(o);

                // Handle Optional<T> fields - only include if they have a value
                if (
                    value != null
                    && value.GetType().IsGenericType
                    && value.GetType().GetGenericTypeDefinition().Name == "Optional`1"
                )
                {
                    var isProvidedProperty = value.GetType().GetProperty("IsProvided");
                    if (isProvidedProperty != null)
                    {
                        var isProvided = (bool)isProvidedProperty.GetValue(value);
                        if (!isProvided)
                        {
                            return (name, value: null); // This will be filtered out by Where clause
                        }

                        // Get the actual value from Optional<T>
                        var valueProperty = value.GetType().GetProperty("Value");
                        if (valueProperty != null)
                        {
                            value = valueProperty.GetValue(value);
                        }
                    }
                }

                return (name, value);
            })
            .Where(p => p.value != null);

        switch (style)
        {
            case SerializationStyle.Simple:
                return string.Join(
                    ",",
                    explode
                        ? properties.Select(p =>
                            $"{p.name}={SerializeValue(p.value, shouldUrlEncode)}"
                        )
                        : properties.Select(p =>
                            $"{p.name},{SerializeValue(p.value, shouldUrlEncode)}"
                        )
                );
            case SerializationStyle.Label:
                return explode
                    ? string.Join(
                        ".",
                        properties.Select(p =>
                            $"{p.name}={SerializeValue(p.value, shouldUrlEncode)}"
                        )
                    )
                    : $"."
                        + string.Join(
                            ",",
                            properties.Select(p =>
                                $"{p.name}={SerializeValue(p.value, shouldUrlEncode)}"
                            )
                        );
            case SerializationStyle.Matrix:
                return explode
                    ? string.Join(
                        $";",
                        properties.Select(p =>
                            $"{p.name}={SerializeValue(p.value, shouldUrlEncode)}"
                        )
                    )
                    : $";{string.Join(",", properties.Select(p => $"{p.name}={SerializeValue(p.value, shouldUrlEncode)}"))}";
            case SerializationStyle.DeepObject:
                return string.Join(
                    "&",
                    properties.Select(p =>
                        $"{key}[{p.name}]={SerializeValue(p.value, shouldUrlEncode)}"
                    )
                );
            default:
                // Form style
                return explode
                    ? string.Join(
                        "&",
                        properties.Select(p =>
                            $"{p.name}={SerializeValue(p.value, shouldUrlEncode)}"
                        )
                    )
                    : $"{key}={string.Join(",", properties.Select((p) => $"{p.name},{SerializeValue(p.value, shouldUrlEncode)}"))}";
        }
    }
}
