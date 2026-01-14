using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities
{
    public class Assignment: BaseAuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string[]? AttachmentUrl { get; set; }

        public DateTime? DueDate { get; set; }
        public int MaxPoints { get; set; } = 100;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }
}
