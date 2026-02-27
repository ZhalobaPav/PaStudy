using Microsoft.AspNetCore.Http;
using PaStudy.Core.Interfaces.Service;

namespace PaStudy.Core.Services;

public class FileService: IFileService
{
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "attachments");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/attachments/{fileName}";
    }
}
