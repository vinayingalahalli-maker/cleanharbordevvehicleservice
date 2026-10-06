namespace VehicleServiceCollectionSdk.Http.Exceptions;

/// <summary>
/// Thrown when a request does not complete within the configured timeout. Distinct from the <see cref="global::System.OperationCanceledException"/> a caller-initiated cancellation raises, so the two are told apart.
/// </summary>
/// <remarks>
/// A transport failure means no HTTP response was ever received, so this exception deliberately
/// carries no status code, body, or headers — the SDK does not invent one. Inspect
/// <see cref="global::System.Exception.InnerException"/> for the underlying transport exception.
/// </remarks>
public class ApiTimeoutException : global::VehicleServiceCollectionSdk.Http.Exceptions.ApiException
{
    public ApiTimeoutException(string message, global::System.Exception? innerException = null)
        : base(message, innerException) { }
}
