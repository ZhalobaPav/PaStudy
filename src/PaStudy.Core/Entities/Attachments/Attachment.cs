using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities.Attachments;

public class Attachment : BaseAuditableEntity
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
}
