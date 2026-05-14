namespace PaStudy.Core.Helpers.DTOs.Course;

public record CreateCourseDto(
    string Title,
    string? Description,
    int? CategoryId
);

public record CourseResponseDto(
    int Id,
    string Title,
    string? Description,
    int? CategoryId,
    DateTimeOffset CreatedAt
);