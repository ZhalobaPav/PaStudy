using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Entities.Base;
using PaStudy.Core.Entities.ConnectionEntities;
using PaStudy.Core.Helpers.Enums;

namespace PaStudy.Core.Entities.Assignments
{
    public class Assignment : BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int MaxPoints { get; set; } = 100;
        public AssignmentType AssignmentType { get; set; } = AssignmentType.Task;

        //Navigation properties 
        public int? SectionId { get; set; }
        public Section Section { get; set; }
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
        public ICollection<Attachment> Attachments { get; set; }
    }
}
