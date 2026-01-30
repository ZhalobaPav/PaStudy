using PaStudy.Core.Helpers.DTOs.Attachment;

namespace PaStudy.Core.Helpers.DTOs.Assignment;

public class CreateAssignmentDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<AttachmentDto> Attachments { get; set; }

    public DateTime? DueDate { get; set; }
    public int MaxPoints { get; set; } = 100;
}
