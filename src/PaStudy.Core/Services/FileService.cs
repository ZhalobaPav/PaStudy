using Microsoft.AspNetCore.Http;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Interfaces.Factories;
using PaStudy.Core.Interfaces.Service;

namespace PaStudy.Core.Services;

public class FileService: IFileService
{
    private readonly IUploadFactory _uploadFactory;
    private readonly string _uploadPath;

    public FileService(IUploadFactory uploadFactory)
    {
        _uploadFactory = uploadFactory;
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "attachments");
    }
    public async Task<CreateAttachmentDto> SaveFileAsync(IFormFile file)
    {
        if (!AttachmentContentTypes.IsImageContentType(file.ContentType) &&
            !AttachmentContentTypes.IsDocumentContentType(file.ContentType))
        {
            throw new ArgumentException("Unsupported file type");
        }

        if (!Directory.Exists(_uploadPath)) Directory.CreateDirectory(_uploadPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_uploadPath, fileName);
        var relativeUrl = $"/uploads/attachments/{fileName}";

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return await _uploadFactory.CreateDtoAsync(file, relativeUrl);
    }
}
