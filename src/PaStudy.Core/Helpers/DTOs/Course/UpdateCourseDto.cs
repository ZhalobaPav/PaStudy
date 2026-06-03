namespace PaStudy.Core.Helpers.DTOs.Course;

public record UpdateCourseDto
{
    public int Id { get; set; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int CategoryId { get; init; }
}
