using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities;

public class Attachment: BaseAuditableEntity
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
}
