namespace PaStudy.Contracts.Commands;

public interface ISendEmailCommand
{
    string To { get; }
    string Subject { get; }
    string Body { get; }
}