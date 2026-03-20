using Microsoft.AspNetCore.Http;
using PaStudy.Core.Helpers.DTOs.Attachment;

namespace PaStudy.Core.Interfaces.Service;

public interface IFileService
{
    Task<CreateAttachmentDto> SaveFileAsync(IFormFile file);
}
