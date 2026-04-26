using Microsoft.AspNetCore.Http;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Interfaces.Factories;
using PaStudy.Core.Interfaces.Service;

namespace PaStudy.Core.Services;

public class FileService: IFileService
{
    private readonly IUploadFactory _uploadFactory;
    private readonly IImageService _imageService;
    private readonly string _uploadPath;

    public FileService(IUploadFactory uploadFactory, IImageService cloudinaryService)
    {
        _uploadFactory = uploadFactory;
        _imageService = cloudinaryService;
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "attachments");
    }
    public async Task<CreateAttachmentDto> SaveFileAsync(IFormFile file)
    {
        if (!AttachmentContentTypes.IsImageContentType(file.ContentType) &&
            !AttachmentContentTypes.IsDocumentContentType(file.ContentType))
        {
            throw new ArgumentException("Unsupported file type");
        }
        using var stream = file.OpenReadStream();
        string finalUrl;
        int? width = null;
        int? height = null;
        if (AttachmentContentTypes.IsImageContentType(file.ContentType))
        {
            var uploadResult = await _imageService.UploadImageAsync(stream, file.FileName);

            finalUrl = uploadResult.Url;
            width = uploadResult.Width;
            height = uploadResult.Height;
        }
        else
        {
            finalUrl = await _imageService.UploadRawFileAsync(stream, file.FileName);
        }
        return await _uploadFactory.CreateDtoAsync(file, finalUrl, width, height);
    }
}
