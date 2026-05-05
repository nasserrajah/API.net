namespace core_first.Application.Common.Exceptions;

public class AppException : Exception
{
    public int StatusCode { get; }
    public string? ErrorDetails { get; }

    public AppException(string message, int statusCode = 400, string? errorDetails = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorDetails = errorDetails;
    }
}