using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Helpers.DTOs.Attachment;

namespace PaStudy.Core.Interfaces.Factories;

public interface IAttachmentFactory
{
    Attachment CreateAttachment(CreateAttachmentDto attachmentDto);
}
