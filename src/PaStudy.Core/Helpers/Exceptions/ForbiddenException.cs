namespace PaStudy.Core.Helpers.Exceptions;

public class ForbiddenException: DomainException
{
    public int StatusCode { get; }
    public ForbiddenException(string? message): base(message ?? "You do not have permission to perform this action.")
    {
        StatusCode = 403;
    }
}
