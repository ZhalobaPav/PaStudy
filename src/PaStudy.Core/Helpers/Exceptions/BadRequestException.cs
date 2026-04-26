namespace PaStudy.Core.Helpers.Exceptions;

public class BadRequestException : DomainException
{
    public BadRequestException(string message) : base(message ?? "Request is bad")
    {
    }
}
