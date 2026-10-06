using System.Net;
using System.Text;

namespace VehicleServiceCollectionSdk.Http.Exceptions;

/// <summary>
/// Exception thrown when an API request receives a non-successful HTTP status code.
/// Captures the status code, headers, and body from the response so they remain
/// available after the underlying <see cref="HttpResponseMessage"/> has been disposed.
/// </summary>
public class ApiException : HttpRequestException
{
    /// <summary>
    /// Gets the response body captured at the time the exception was thrown.
    /// </summary>
    public string? Body { get; }

    /// <summary>
    /// Gets the response headers (including content headers) captured at the time the exception was thrown.
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Headers { get; }

    /// <summary>
    /// Gets a detached, in-memory copy of the response that triggered this exception.
    /// The live <see cref="HttpResponseMessage"/> is disposed on the error path; this copy is
    /// safe to read repeatedly and holds no network resources.
    /// </summary>
    [Obsolete(
        "Response is a buffered copy; the live HttpResponseMessage is disposed. Use StatusCode, Body, and Headers instead."
    )]
    public HttpResponseMessage Response { get; }

    public ApiException(
        HttpStatusCode statusCode,
        string? body,
        IReadOnlyDictionary<string, string[]> headers
    )
        : this(
            $"Response status code does not indicate success: {(int)statusCode} ({statusCode}).",
            statusCode,
            body,
            headers
        ) { }

    public ApiException(
        string message,
        HttpStatusCode statusCode,
        string? body,
        IReadOnlyDictionary<string, string[]> headers
    )
        : base(message, null, statusCode)
    {
        Body = body;
        Headers = headers;
#pragma warning disable CS0618 // ApiException populates the obsolete Response for backward compatibility.
        Response = BuildBufferedResponse(statusCode, body, headers);
#pragma warning restore CS0618
    }

    /// <summary>
    /// Transport-failure constructor. Used by the transport-failure subclasses, which are
    /// <c>ApiException</c>s so an existing <c>catch (ApiException)</c> still fires, but which never
    /// saw a response: <see cref="StatusCode"/> stays null and the obsolete <see cref="Response"/> is
    /// null rather than a made-up 200, because a fabricated status would be indistinguishable from a
    /// real one (FSDK-1585).
    /// </summary>
    protected ApiException(string message, Exception? innerException)
        : base(message, innerException)
    {
        Body = null;
        Headers = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
#pragma warning disable CS0618 // No response was received, so there is nothing to buffer.
        Response = null!;
#pragma warning restore CS0618
    }

    private static HttpResponseMessage BuildBufferedResponse(
        HttpStatusCode statusCode,
        string? body,
        IReadOnlyDictionary<string, string[]> headers
    )
    {
        var content = new ByteArrayContent(Encoding.UTF8.GetBytes(body ?? string.Empty));
        var response = new HttpResponseMessage(statusCode) { Content = content };
        foreach (var header in headers)
        {
            // Response headers reject content headers (e.g. Content-Type); fall back to content headers.
            if (!response.Headers.TryAddWithoutValidation(header.Key, header.Value))
            {
                content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
        return response;
    }

    /// <summary>
    /// Reads the body, captures the status code and headers, disposes <paramref name="response"/>,
    /// and builds an <see cref="ApiException"/> from the captured values.
    /// </summary>
    internal static async Task<ApiException> FromResponseAsync(
        HttpResponseMessage response,
        string? message = null
    )
    {
        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var (statusCode, headers) = CaptureAndDispose(response);
        return message is null
            ? new ApiException(statusCode, body, headers)
            : new ApiException(message, statusCode, body, headers);
    }

    /// <summary>
    /// Captures the status code and headers from a response whose body has already been read,
    /// then disposes it so its network resources are released.
    /// </summary>
    internal static (
        HttpStatusCode StatusCode,
        IReadOnlyDictionary<string, string[]> Headers
    ) CaptureAndDispose(HttpResponseMessage response)
    {
        var statusCode = response.StatusCode;
        // HTTP header names are case-insensitive; group case-folded (last value wins) so consumer
        // lookups like Headers["content-type"] hit and a duplicated key can't crash error handling.
        var headers = response
            .Headers.Concat(response.Content.Headers)
            .GroupBy(header => header.Key, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group.Last().Value.ToArray(),
                StringComparer.OrdinalIgnoreCase
            );
        response.Dispose();
        return (statusCode, headers);
    }
}
