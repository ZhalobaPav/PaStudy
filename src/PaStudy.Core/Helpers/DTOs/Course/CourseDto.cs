using PaStudy.Core.Helpers.DTOs.Teacher;

namespace PaStudy.Core.Helpers.DTOs.Course;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? CategoryName { get; set; }
    public ICollection<BriefTeacherDto>? Teachers { get; set; }
}
