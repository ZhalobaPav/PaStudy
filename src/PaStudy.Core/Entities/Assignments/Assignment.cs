using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Entities.Base;
using PaStudy.Core.Helpers.Enums;
using SubmissionEntity = PaStudy.Core.Entities.Assignments.Submission.Submission;
namespace PaStudy.Core.Entities.Assignments
{
    public abstract class Assignment : BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartDate { get; set; }
        public int MaxPoints { get; set; } = 100;
        public AssignmentType AssignmentType { get; set; } = AssignmentType.Task;

        //Navigation properties 
        public int? SectionId { get; set; }
        public Section Section { get; set; }
        public ICollection<SubmissionEntity> Submissions { get; set; } = new List<SubmissionEntity>();
        public ICollection<Attachment> Attachments { get; set; }
    }
}
