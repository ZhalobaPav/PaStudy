using Microsoft.AspNetCore.Http;

namespace PaStudy.Core.Helpers.DTOs.Attachment;

public class CreateAttachmentRequest
{
    public IFormFile File { get; set; } = null!;
    public int AssignmentId { get; set; }
}
