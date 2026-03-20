using Microsoft.AspNetCore.Http;
using PaStudy.Core.Helpers.DTOs.Attachment;

namespace PaStudy.Core.Interfaces.Factories;

public interface IUploadFactory
{
    Task<CreateAttachmentDto> CreateDtoAsync(IFormFile file, string savedUrl);
}
