using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using PaStudy.Core.Helpers.ConfigurationModels;
using System.Security.Principal;

namespace PaStudy.Core.Services;

public class CloudinaryService: IImageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<(string Url, int Width, int Height)> UploadImageAsync(Stream fileStream, string fileName)
    {
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(fileName, fileStream),
            Folder = "pastudy_attachments",
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
            throw new Exception(uploadResult.Error.Message);

        return (
            uploadResult.SecureUrl.ToString(),
            uploadResult.Width,
            uploadResult.Height
        );
    }
    public async Task<string> UploadRawFileAsync(Stream fileStream, string fileName)
    {
        var uploadParams = new RawUploadParams()
        {
            File = new FileDescription(fileName, fileStream),
            Folder = "pastudy_documents",
            UseFilename = true,
            UniqueFilename = true
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
            throw new Exception(uploadResult.Error.Message);

        return uploadResult.SecureUrl.ToString();
    }
}
public interface IImageService
{
    Task<(string Url, int Width, int Height)> UploadImageAsync(Stream fileStream, string fileName);
    Task<string> UploadRawFileAsync(Stream fileStream, string fileName);
}