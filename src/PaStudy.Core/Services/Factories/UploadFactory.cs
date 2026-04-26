using Microsoft.AspNetCore.Http;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Interfaces.Factories;

namespace PaStudy.Core.Services.Factories;

public class UploadFactory: IUploadFactory
{
    public Task<CreateAttachmentDto> CreateDtoAsync(IFormFile file, string savedUrl, int? width = null, int? height = null)
    {
        var dto = new CreateAttachmentDto
        {
            FileName = file.FileName,
            FileUrl = savedUrl,
            ContentType = file.ContentType
        };
        if (width.HasValue && height.HasValue)
        {
            dto.ImageInfo = new ImageAttachmentInfo(width.Value, height.Value);
        }

        return Task.FromResult(dto);
    }
}
