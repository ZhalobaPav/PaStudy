using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities.Notification;

public enum NotificationType
{
    CourseInvitation = 1,
    NewAssignment = 2,
    GradeReceived = 3,
    GeneralAnnouncement = 4,
    SubmissionUploaded = 5
}

public class Notification : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;

    public NotificationType Type { get; set; }

    public string? ClickActionUrl { get; set; }
    public string? RecipientUserId { get; set; }

    public int? CourseId { get; set; }
    public Course? Course { get; set; }

    public bool IsRead { get; set; } = false;
}