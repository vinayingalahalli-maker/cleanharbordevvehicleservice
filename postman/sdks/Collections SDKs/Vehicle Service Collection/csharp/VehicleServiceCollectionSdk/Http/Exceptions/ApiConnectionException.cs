namespace VehicleServiceCollectionSdk.Http.Exceptions;

/// <summary>
/// Thrown when the request never reached the server — a refused connection, an unresolvable host, a TLS failure or a reset. Replaces the raw <see cref="global::System.Net.Http.HttpRequestException"/> the transport raises, which is kept as the inner exception.
/// </summary>
/// <remarks>
/// A transport failure means no HTTP response was ever received, so this exception deliberately
/// carries no status code, body, or headers — the SDK does not invent one. Inspect
/// <see cref="global::System.Exception.InnerException"/> for the underlying transport exception.
/// </remarks>
public class ApiConnectionException
    : global::VehicleServiceCollectionSdk.Http.Exceptions.ApiException
{
    public ApiConnectionException(string message, global::System.Exception? innerException = null)
        : base(message, innerException) { }
}
