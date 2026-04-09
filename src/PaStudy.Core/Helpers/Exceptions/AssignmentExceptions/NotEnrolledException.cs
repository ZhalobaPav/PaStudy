namespace PaStudy.Core.Helpers.Exceptions.AssignmentExceptions;

public class NotEnrolledException: DomainException
{
    public int StatusCode { get; }
    public NotEnrolledException(string? message) : base(message ?? "You are not enrolled on this course")
    {
        StatusCode = 404;
    }
}
