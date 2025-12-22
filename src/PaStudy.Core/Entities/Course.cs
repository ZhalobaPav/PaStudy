using PaStudy.Core.Entities.Base;
using PaStudy.Core.Entities.ConnectionEntities;

namespace PaStudy.Core.Entities;
public class Course: BaseAuditableEntity
{
    public string Title { get; set; }
    public string? Description {  get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    public ICollection<TeacherCourses> TeacherCourses { get; set; }
}

