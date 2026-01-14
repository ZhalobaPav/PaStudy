namespace PaStudy.Core.Helpers.Exceptions;

public class NotFoundException: DomainException
{
    public NotFoundException(string? message): base(message ?? "The requested resource was not found.")
    {
        
    }
}
