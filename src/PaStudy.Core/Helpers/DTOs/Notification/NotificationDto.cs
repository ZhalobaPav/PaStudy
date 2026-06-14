using PaStudy.Core.Entities.Notification;

namespace PaStudy.Core.Helpers.DTOs.Notification;

public record NotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;

    public NotificationType Type { get; set; }
    public InvitationStatus? InvitationStatus { get; set; }

    public string? ClickActionUrl { get; set; }
    public string? RecipientUserId { get; set; }

    public int? CourseId { get; set; }

    public bool IsRead { get; set; } = false;

}

public record SendInviteDto(int CourseId, string RecipientUserId, string CourseName);
public record RespondToInviteDto(bool Accept);