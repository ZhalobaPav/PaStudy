using MailKit.Net.Smtp;
using MassTransit;
using MimeKit;
using PaStudy.Contracts.Commands;

namespace PaStudy.NotificationService.Consumers;

public class EmailConsumer : IConsumer<SendEmailCommand>
{
    public async Task Consume(ConsumeContext<SendEmailCommand> context)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("PaStudy Admin", "admin@pastudy.com"));
        message.To.Add(new MailboxAddress("", context.Message.To));
        message.Subject = context.Message.Subject;

        message.Body = new TextPart(context.Message.IsHtml ? "html" : "plain")
        {
            Text = context.Message.Body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync("localhost", 1025, false);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        Console.WriteLine($"[LOG] Email sent to {context.Message.To}: {context.Message.Subject}");
    }
}
