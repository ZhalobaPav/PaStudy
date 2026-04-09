using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Entities.Base;
using PaStudy.Core.Helpers.Enums;

namespace PaStudy.Core.Entities.Assignments.Submission;

public abstract class Submission : BaseAuditableEntity
{
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public decimal? Grade { get; set; }
    public string? TeacherFeedback { get; set; }
    public DateTime? GradedAt { get; set; }

    // Navigation properties
    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public SubmissionStatus Status { get; set; }
}
