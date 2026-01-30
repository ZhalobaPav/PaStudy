using PaStudy.Core.Helpers.DTOs.Attachment;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.DTOs.Assignment;

public class AssignmentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public ImmutableArray<AttachmentDto?> Attachments { get; set; } = new ImmutableArray<AttachmentDto?>();
    public int MaxPoints { get; set; } = 100;
}
