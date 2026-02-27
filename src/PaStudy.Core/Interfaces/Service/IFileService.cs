using Microsoft.AspNetCore.Http;

namespace PaStudy.Core.Interfaces.Service;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);
}
