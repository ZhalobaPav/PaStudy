namespace PaStudy.Core.Helpers.Exceptions;

public abstract class DomainException: Exception
{
    protected DomainException(string message): base(message)
    {
        
    }
}
