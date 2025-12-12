using PaStudy.Core.Entities.Base;
using PaStudy.Core.Entities.ConnectionEntities;

namespace PaStudy.Core.Entities;
public class Student: BaseAuditableEntity
{
    public string UserId { get; set; }

    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
}

