using PaStudy.Core.Helpers.DTOs.Teacher;

namespace PaStudy.Core.Helpers.DTOs.Course;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? CategoryName { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsEnrolled { get; set; } = false;
    public bool? IsTeaching { get; set; } = false;
    public ICollection<BriefTeacherDto>? Teachers { get; set; }
}
