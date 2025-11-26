using PaStudy.Core.Entities.Base;
using PaStudy.Core.Entities.ConnectionEntities;

namespace PaStudy.Core.Entities;
public class Course: BaseAuditableEntity
{
    public string Title { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    public ICollection<TeacherCourses> TeacherCourses { get; set; }
}

