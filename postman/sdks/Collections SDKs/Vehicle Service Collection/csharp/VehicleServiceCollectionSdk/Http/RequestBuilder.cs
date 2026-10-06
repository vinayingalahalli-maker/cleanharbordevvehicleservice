using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using VehicleServiceCollectionSdk.Http.Extensions;
using VehicleServiceCollectionSdk.Http.Serialization;

namespace VehicleServiceCollectionSdk.Http;

/// <summary>
/// A builder for creating <see cref="HttpRequestMessage"/> instances with full support for serialization.
/// </summary>
public class RequestBuilder
{
    private readonly string _urlTemplate;

    private readonly HttpMethod _httpMethod;

    private readonly Dictionary<string, string> _pathParameters = new();
    private readonly List<string> _queryParameters = new();
    private readonly List<string> _cookieParameters = new();
    private readonly Dictionary<string, string> _headers = new();
    private readonly List<ErrorMapping> _errorMappings = new();
    private ErrorMapping? _defaultErrorMapping;

    private HttpContent? _content;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestBuilder"/> class.
    /// </summary>
    /// <param name="httpMethod">The HTTP method to use for the request.</param>
    /// <param name="urlTemplate">The URL template to use for the request. Should have path parameters as placeholders surrounded by brackets (Eg. "/users/{id}", where "id" is a path parameter).</param>
    public RequestBuilder(HttpMethod httpMethod, string urlTemplate)
    {
        _httpMethod = httpMethod;
        _urlTemplate = urlTemplate;
    }

    /// <summary>
    /// Sets a path parameter. If the parameter is not present in the URL template, it will be ignored.
    /// </summary>
    public RequestBuilder SetPathParameter(
        string key,
        object value,
        PathSerializationStyle style = PathSerializationStyle.Simple,
        bool explode = false
    )
    {
        var serializedValue = Serializer.Serialize(key, value, (SerializationStyle)style, explode);
        _pathParameters.Add(key, serializedValue);
        return this;
    }

    /// <summary>
    /// Sets a query parameter.
    /// </summary>
    public RequestBuilder SetQueryParameter(
        string key,
        object? value,
        QuerySerializationStyle style = QuerySerializationStyle.Form,
        bool explode = true
    )
    {
        var serializedValue = Serializer.Serialize(key, value, (SerializationStyle)style, explode);
        if (!string.IsNullOrEmpty(serializedValue))
        {
            _queryParameters.Add(serializedValue);
        }
        return this;
    }

    /// <summary>
    /// Sets a query parameter if the value is not null.
    /// </summary>
    public RequestBuilder SetOptionalQueryParameter(
        string key,
        object? value,
        QuerySerializationStyle style = QuerySerializationStyle.Form,
        bool explode = true
    )
    {
        if (value is not null)
        {
            SetQueryParameter(key, value, style, explode);
        }
        return this;
    }

    /// <summary>
    /// Sets a header.
    /// </summary>
    public RequestBuilder SetHeader(string key, object? value, bool explode = false)
    {
        var serializedValue = Serializer.Serialize(
            key,
            value,
            SerializationStyle.Simple,
            explode,
            false
        );
        if (!string.IsNullOrEmpty(serializedValue))
        {
            _headers.Add(key, serializedValue);
        }
        return this;
    }

    /// <summary>
    /// Sets a header if the value is not null.
    /// </summary>
    public RequestBuilder SetOptionalHeader(string key, object? value, bool explode = false)
    {
        if (value is not null)
        {
            SetHeader(key, value, explode);
        }
        return this;
    }

    /// <summary>
    /// Sets a cookie parameter. Multiple calls accumulate cookies combined into a single Cookie header.
    /// Arrays with explode=true produce separate name=value pairs (e.g. "key=v1; key=v2").
    /// Arrays with explode=false produce a comma-separated list (e.g. "key=v1,v2,v3").
    /// </summary>
    public RequestBuilder SetCookieParameter(string key, object? value, bool explode = true)
    {
        var serialized = Serializer.Serialize(key, value, SerializationStyle.Form, explode, false);
        if (!string.IsNullOrEmpty(serialized))
        {
            _cookieParameters.AddRange(serialized.Split('&'));
        }
        return this;
    }

    /// <summary>
    /// Sets a cookie parameter if the value is not null.
    /// </summary>
    public RequestBuilder SetOptionalCookieParameter(string key, object? value, bool explode = true)
    {
        if (value is not null)
        {
            SetCookieParameter(key, value, explode);
        }
        return this;
    }

    /// <summary>
    /// Sets the content of the request as JSON.
    /// </summary>
    public RequestBuilder SetContentAsJson(
        object content,
        JsonSerializerOptions? options = null,
        MediaTypeHeaderValue? mediaType = null
    )
    {
        _content = JsonContent.Create(content, mediaType, options);
        return this;
    }

    /// <summary>
    /// Sets the content of the request as Text.
    /// </summary>
    public RequestBuilder SetContentAsText(
        string content,
        string mediaType,
        Encoding? encoding = null
    )
    {
        encoding ??= Encoding.UTF8;
        _content = new StringContent(content, encoding, mediaType);
        return this;
    }

    /// <summary>
    /// Sets the content of the request as Binary.
    /// </summary>
    public RequestBuilder SetContentAsBinary(byte[] content, MediaTypeHeaderValue mediaType)
    {
        _content = new ByteArrayContent(content);
        _content.Headers.ContentType = mediaType;
        return this;
    }

    /// <summary>
    /// Sets the content of the request as application/x-www-form-urlencoded.
    /// </summary>
    public RequestBuilder SetUrlEncodedContent(
        object content,
        JsonSerializerOptions? options = null
    )
    {
        var jsonContent = JsonSerializer.Serialize(content, options);

        // Deserializing into Dictionary<string, string> asked System.Text.Json to read every
        // member as a string, so an array, an object — even a plain number or bool — threw
        // before the request was built. Walking the JSON tree instead lets each kind decide
        // how it renders.
        if (JsonNode.Parse(jsonContent) is not JsonObject root)
        {
            throw new ArgumentException("Invalid content for form-urlencoded content type.");
        }

        // A sequence of pairs rather than a dictionary: an array field has to repeat its key
        // (`tags=a&tags=b`), which a dictionary cannot represent.
        var fields = new List<KeyValuePair<string, string>>();
        foreach (var property in root)
        {
            AddUrlEncodedField(fields, property.Key, property.Value);
        }

        _content = new FormUrlEncodedContent(fields);
        return this;
    }

    /// <summary>
    /// Expands one JSON member into the form fields it contributes, following the OpenAPI
    /// default encoding for application/x-www-form-urlencoded (style: form, explode: true).
    /// </summary>
    private static void AddUrlEncodedField(
        List<KeyValuePair<string, string>> fields,
        string key,
        JsonNode? node
    )
    {
        switch (node)
        {
            // An absent member is not a field. Emitting `key=` would tell the server the caller
            // sent an empty string, which is a different statement from "unset".
            case null:
                return;
            case JsonArray array:
                foreach (var element in array)
                {
                    AddUrlEncodedField(fields, key, element);
                }
                return;
            // Brackets keep the parent name, matching how this SDK's own deepObject query
            // serialization expresses nesting in a key/value context.
            case JsonObject nested:
                foreach (var property in nested)
                {
                    AddUrlEncodedField(fields, $"{key}[{property.Key}]", property.Value);
                }
                return;
            default:
                fields.Add(new KeyValuePair<string, string>(key, ToUrlEncodedValue(node)));
                return;
        }
    }

    private static string ToUrlEncodedValue(JsonNode node)
    {
        // A string has to be unwrapped rather than rendered through ToJsonString(), which would
        // keep the surrounding quotes; every other scalar renders as its JSON text. Matched with
        // TryGetValue rather than GetValueKind() so this still compiles on net6.0/netstandard2.0,
        // where JsonNode.GetValueKind() does not exist.
        if (node is JsonValue value && value.TryGetValue<string>(out var text) && text != null)
        {
            return text;
        }

        return node.ToJsonString();
    }

    /// <summary>
    /// Sets the content of the request as multipart/form-data.
    /// </summary>
    public RequestBuilder SetContentAsMultipartFormData(
        object content,
        JsonSerializerOptions? options = null
    )
    {
        _content = new MultipartFormDataContent().AddObject(content, options);
        return this;
    }

    private string BuildUrl()
    {
        var url = _urlTemplate;
        foreach (var (key, value) in _pathParameters)
        {
            url = url.Replace($"{{{key}}}", value);
        }

        if (_queryParameters.Any())
        {
            url += "?" + string.Join("&", _queryParameters);
        }

        return url;
    }

    /// <summary>
    /// Adds a mapping between an HTTP status code/content type combination and the corresponding error model and exception type.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to map (e.g., 400, 404, 500).</param>
    /// <param name="contentType">The content type of the error response (e.g., "application/json").</param>
    /// <param name="targetType">The type to deserialize the error response body into.</param>
    /// <param name="exceptionType">The exception type to throw when this error occurs. Must extend Exception.</param>
    public RequestBuilder AddError(
        int statusCode,
        string contentType,
        Type targetType,
        Type exceptionType
    )
    {
        var errorMapping = new ErrorMapping
        {
            StatusCode = statusCode,
            ContentType = contentType,
            TargetType = targetType,
            ExceptionType = exceptionType,
        };

        if (statusCode == Request.NoStatusCode)
        {
            _defaultErrorMapping = errorMapping;
        }
        else
        {
            _errorMappings.Add(errorMapping);
        }

        return this;
    }

    /// <summary>
    ///  Sets the default error mapping to be thrown when no other error can be matched
    /// </summary>
    public RequestBuilder AddDefaultError(string contentType, Type targetType, Type exceptionType)
    {
        return AddError(Request.NoStatusCode, contentType, targetType, exceptionType);
    }

    /// <summary>
    /// Adds a header to the request, or to its content when the header belongs there.
    /// </summary>
    /// <remarks>
    /// `HttpRequestMessage.Headers.Add` throws for a CONTENT header -- `Content-Encoding`,
    /// `Content-Type`, `Content-Disposition` and friends live on `HttpContent`, and .NET rejects
    /// them with `InvalidOperationException: Misused header name`. It also throws
    /// `FormatException` for a value its parser rejects for that header, which a caller-supplied
    /// value can be. Both took out a generated SDK at runtime, so try the request collection
    /// without validation and fall through to the content one.
    ///
    /// The content collection is REPLACED, not appended to. `HttpContent` sets its own
    /// `Content-Type` on construction, so appending produced
    /// `text/plain; charset=utf-8, application/vnd.custom+json` -- not a valid media type, and sent
    /// silently where `.Add` used to throw. A caller who names a content header explicitly means to
    /// override it.
    ///
    /// One residual gap: a request with no body has no `Content`, so a content header on such a
    /// request is DROPPED rather than sent. That is deliberate -- there is nowhere valid to put it
    /// -- but it is silent, so a caller setting `Content-Type` on a bodyless GET will not see it on
    /// the wire and gets no error either.
    ///
    /// CR and LF are rejected up front. `TryAddWithoutValidation` skips the newline check as well
    /// as the format one, and a newline in a header value SPLITS the request -- everything after it
    /// is read by the server as further headers (CWE-113). `.Add` refused such a value outright
    /// ("New-line characters are not allowed in header values"), so this keeps that guarantee while
    /// still letting through the values whose only problem is the header-specific parser.
    /// </remarks>
    private static void AddHeader(HttpRequestMessage requestMessage, string name, string? value)
    {
        // IndexOf(char), not Contains(char): the latter does not exist on net462/netstandard2.0.
        if (value != null && (value.IndexOf('\r') >= 0 || value.IndexOf('\n') >= 0))
        {
            throw new ArgumentException(
                $"Header '{name}' has a value containing a CR or LF character, which would split the request.",
                nameof(value)
            );
        }
        if (requestMessage.Headers.TryAddWithoutValidation(name, value))
        {
            return;
        }
        requestMessage.Content?.Headers.Remove(name);
        requestMessage.Content?.Headers.TryAddWithoutValidation(name, value);
    }

    /// <summary>
    /// Builds the <see cref="HttpRequestMessage"/> instance.
    /// </summary>
    public HttpRequestMessage BuildHttpRequestMessage()
    {
        var requestMessage = new HttpRequestMessage(_httpMethod, BuildUrl()) { Content = _content };

        foreach (var (key, value) in _headers)
        {
            AddHeader(requestMessage, key, value);
        }

        if (_cookieParameters.Count > 0)
        {
            requestMessage.Headers.Add("Cookie", string.Join("; ", _cookieParameters));
        }

        return requestMessage;
    }

    /// <summary>
    /// Builds the <see cref="Request"/> object containing all request information.
    /// </summary>
    public Request Build()
    {
        return new Request
        {
            Url = BuildUrl(),
            HttpMethod = _httpMethod,
            Headers = new Dictionary<string, string>(_headers),
            Content = _content,
            ErrorMappings = new List<ErrorMapping>(_errorMappings),
            DefaultErrorMapping = _defaultErrorMapping,
            HttpRequestMessage = BuildHttpRequestMessage(),
        };
    }
}
