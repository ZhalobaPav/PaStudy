using PaStudy.Core.Entities.Base;
using PaStudy.Core.Helpers.Enums;

namespace PaStudy.Core.Entities.ConnectionEntities;
public class Enrollment: BaseAuditableEntity
{
    public decimal? FinalGrade { get; set; }
    public double Progress { get; set; } // 0..100%
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

    // Navigation properties
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
}

