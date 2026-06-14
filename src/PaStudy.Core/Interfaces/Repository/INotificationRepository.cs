using PaStudy.Contracts.Commands;
using PaStudy.Core.Entities.Notification;
using PaStudy.Core.Helpers.DTOs.Notification;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Repository;

public interface INotificationRepository
{
    Task<Notification> AddNotificationAsync(CreateNotificationDto dto);
    Task<ImmutableArray<NotificationDto>> GetNotifications(CancellationToken ct);
    Task<(bool Success, int? CourseId)> RespondToInvitationAsync(
        int notificationId,
        string userId,
        bool accept,
        CancellationToken ct);
}
