using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities
{
    public class Assignment: BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int MaxPoints { get; set; } = 100;

        //Navigation properties 
        public int? SectionId { get; set; }
        public Section Section { get; set; }
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
        public ICollection<Attachment> Attachments { get; set; }
    }
}
