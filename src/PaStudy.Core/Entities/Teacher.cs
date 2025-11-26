using PaStudy.Core.Entities.Base;
using PaStudy.Core.Entities.ConnectionEntities;

namespace PaStudy.Core.Entities;
public class Teacher : BaseAuditableEntity
{
    public string UserId { get; set; }

    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public Group GroupOfCurator { get; set; }
    public int GroupId { get; set; }
    public ICollection<TeacherCourses> TeacherCourses { get; set; }
}

