using PaStudy.Core.Entities.Attachments;

namespace PaStudy.Core.Entities.Assignments.Submission;

public class TaskSubmission: Submission
{
    public string? StudentNotes { get; set; }
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
