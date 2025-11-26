using PaStudy.Core.Entities.Base;
using PaStudy.Core.Entities.ConnectionEntities;

namespace PaStudy.Core.Entities;
public class Group: BaseAuditableEntity
{
    public string GroupNumber { get; set; }
    public string InstitutionNumber { get; set; }
    public int Year { get; set; }
    public string Faculty { get; set; } 
    public string Speciality { get; set; }

    // Navigation properties
    public ICollection<Student> Students { get; set; }
    public Teacher CuratorOfGroup { get; set; }
    public int CuratorOfGroupId { get; set; }
}

