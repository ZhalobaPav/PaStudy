using Microsoft.EntityFrameworkCore;
using PaStudy.Contracts.Commands;
using PaStudy.Core.Entities.Notification;
using PaStudy.Core.Helpers.DTOs.Notification;
using PaStudy.Core.Helpers.StaticData;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using System.Collections.Immutable;

namespace PaStudy.Infrastructure.Repositories;

public class NotificationRepository: INotificationRepository
{
    private readonly PaStudyDbContext _context;

    public NotificationRepository(PaStudyDbContext context)
    {
        _context = context;
    }
    public async Task<ImmutableArray<NotificationDto>> GetNotifications(CancellationToken ct)
    {
        return await _context.Set<Notification>().Where(n => n.RecipientUserId == null).OrderByDescending(n => n.Id).Take(BaseConsts.TakeNotificationNumber)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                ClickActionUrl = n.ClickActionUrl,
                InvitationStatus = n.InvitationStatus,
                RecipientUserId = n.RecipientUserId,
                CourseId = n.CourseId,
                IsRead = n.IsRead
            }).ToImmutableArrayAsync(ct);
    }
    public async Task<Notification> AddNotificationAsync(CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            Title = dto.Title,
            Message = dto.Message,
            Type = dto.Type,
            InvitationStatus = dto.Type == NotificationType.CourseInvitation ? InvitationStatus.Pending : null,
            ClickActionUrl = dto.ClickActionUrl,
            RecipientUserId = dto.RecipientUserId,
            CourseId = dto.CourseId,
            IsRead = dto.IsRead,
            Created = DateTime.UtcNow,
            CreatedBy = dto.RecipientUserId
        };
        await _context.Set<Notification>().AddAsync(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    // PaStudy.Infrastructure.Repositories.NotificationRepository.cs

    public async Task<(bool Success, int? CourseId)> RespondToInvitationAsync(
        int notificationId,
        string userId,
        bool accept,
        CancellationToken ct)
    {
        var notification = await _context.Set<Notification>()
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.RecipientUserId == userId, ct);

        // Перевіряємо, чи існує нотифікація і чи це дійсно запрошення зі статусом Pending
        if (notification == null ||
            notification.Type != NotificationType.CourseInvitation ||
            notification.InvitationStatus != InvitationStatus.Pending)
        {
            return (false, null);
        }

        // Змінюємо статус
        notification.InvitationStatus = accept ? InvitationStatus.Accepted : InvitationStatus.Declined;
        notification.IsRead = true; // Відразу позначаємо як прочитане

        await _context.SaveChangesAsync(ct);

        return (true, notification.CourseId);
    }
}
