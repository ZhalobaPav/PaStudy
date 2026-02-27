using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Helpers.DTOs.Attachment;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Service;

public interface IAttachmentService
{
    Task<ImmutableArray<Attachment>> UploadAttachmentsAsync(IEnumerable<CreateAttachmentDto> attachments);
}
