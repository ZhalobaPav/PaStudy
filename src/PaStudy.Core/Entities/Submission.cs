using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities;

public class Submission: BaseAuditableEntity
{
    public string? Content { get; set; }
    public string? FileUrl { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public decimal? Grade { get; set; }
    public string? TeacherFeedback { get; set; }
    public DateTime? GradedAt { get; set; }
    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
}
