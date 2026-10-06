using VehicleServiceCollectionSdk.Http.Exceptions;

namespace VehicleServiceCollectionSdk.Http.Extensions;

/// <summary>
/// Extension methods for HttpResponseMessage to add custom validation and error handling.
/// </summary>
public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Ensures the HTTP response has a successful status code (2xx), throwing ApiException if not.
    /// On failure the response body and headers are captured and the response is disposed, so no
    /// live HttpResponseMessage is leaked or exposed on the exception.
    /// </summary>
    /// <param name="response">The HTTP response to validate.</param>
    /// <returns>The same response if successful, for method chaining.</returns>
    /// <exception cref="ApiException">Thrown when the response status code indicates failure.</exception>
    public static async Task<HttpResponseMessage> EnsureSuccessfulResponseAsync(
        this HttpResponseMessage response
    )
    {
        if (!response.IsSuccessStatusCode)
        {
            throw await ApiException.FromResponseAsync(response).ConfigureAwait(false);
        }
        return response;
    }
}
