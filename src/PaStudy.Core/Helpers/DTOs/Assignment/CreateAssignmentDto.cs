namespace PaStudy.Core.Helpers.DTOs.Assignment;

public class CreateAssignmentDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AttachmentUrl { get; set; }

    public DateTime? DueDate { get; set; }
    public int MaxPoints { get; set; } = 100;
    public int CourseId { get; set; }
}
