using MassTransit;
using Microsoft.AspNetCore.SignalR;
using PaStudy.Contracts.Commands;
using PaStudy.Core.Interfaces.Repository;

namespace PaStudy.NotificationService.Consumers;

public class SubmissionGradedConsumer : IConsumer<SubmissionGradedEvent>
{
    private readonly INotificationRepository _notificationRepository;

    public SubmissionGradedConsumer(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext)
    {
        _notificationRepository = notificationRepository;
    }
    public async Task Consume(ConsumeContext<SubmissionGradedEvent> context)
    {
        var ev = context.Message;
        var notification = await _notificationRepository.AddNotificationAsync(ev);
        await _hubContext.Clients.User(ev.StudentUserId).SendAsync("ReceiveNotification", new
        {
            id = notification.Id,
            title = notification.Title,
            message = notification.Message,
            clickActionUrl = notification.ClickActionUrl,
            createdAt = notification.Created
        });
    }
}
