using Microsoft.AspNetCore.Http;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Interfaces.Factories;

namespace PaStudy.Core.Services.Factories;

public class UploadFactory: IUploadFactory
{
    public async Task<CreateAttachmentDto> CreateDtoAsync(IFormFile file, string savedUrl)
    {
        var dto = new CreateAttachmentDto
        {
            FileName = file.FileName,
            FileUrl = savedUrl,
            ContentType = file.ContentType
        };

        if (AttachmentContentTypes.IsImageContentType(file.ContentType))
        {
            using var image = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream());
            dto.ImageInfo = new ImageAttachmentInfo(image.Width, image.Height);
        }

        return dto;
    }
}
