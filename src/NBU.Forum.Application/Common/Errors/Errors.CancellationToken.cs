namespace NBU.Forum.Application.Common.Errors;

using ErrorOr;

public static partial class Errors
{
    public static class CancellationToken
    {
        public static readonly Error Cancelled = Error.Failure("Cancellation.Token",
            "The operation was cancelled.");
    }
}
