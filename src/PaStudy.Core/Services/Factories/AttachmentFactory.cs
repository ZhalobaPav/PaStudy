using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.StaticData;
using PaStudy.Core.Interfaces.Factories;

namespace PaStudy.Core.Services.Factories;

public class AttachmentFactory: IAttachmentFactory
{
    public Attachment CreateAttachment(CreateAttachmentDto attachmentDto)
    {
        return attachmentDto.ContentType switch
        {
            var ct when AttachmentContentTypes.IsImageContentType(ct) => CreateImage(attachmentDto),
            var ct when AttachmentContentTypes.IsDocumentContentType(ct) => CreateDocument(attachmentDto),
            _ => throw new NotSupportedException($"Content type '{attachmentDto.ContentType}' is not supported.")
        };
    }

    public ImageAttachment CreateImage(CreateAttachmentDto attachmentDto) 
    {
        return new ImageAttachment
        {
            FileName = attachmentDto.FileName,
            FileUrl = attachmentDto.FileUrl,
            ContentType = attachmentDto.ContentType,
            Width = attachmentDto.ImageInfo?.Width ?? ImageConfiguration.DefaultImageWidth,
            Height = attachmentDto.ImageInfo?.Height ?? ImageConfiguration.DefaultImageHeight,
        };
    }

    public Attachment CreateDocument(CreateAttachmentDto attachmentDto)
    {
        return new Attachment
        {
            FileName = attachmentDto.FileName,
            FileUrl = attachmentDto.FileUrl,
            ContentType = attachmentDto.ContentType,
            FileSize = new FileInfo(attachmentDto.FileUrl).Length
        };
    }
}
