using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities;

public class Section: BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public ICollection<Assignment> Assignments { get; set; }
}
