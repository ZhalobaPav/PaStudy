namespace PaStudy.Core.Helpers.DTOs.Section;

public class CreateSectionDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CourseId { get; set; }
}
