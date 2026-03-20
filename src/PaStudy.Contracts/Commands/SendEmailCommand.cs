namespace PaStudy.Contracts.Commands;

public record SendEmailCommand(
    string To,
    string Subject,
    string Body,
    bool IsHtml = true
);