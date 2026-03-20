using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Core.Services;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;

namespace PavStudy.API.Endpoints;

public class Files : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var group = app.MapGroup(this)
            .RequireAuthorization()
            .DisableAntiforgery();

        group.MapPost("upload", Upload);

        group.MapPost("upload-multiple", async (IFormFileCollection files, IFileService fileService) =>
        {
            var results = new List<CreateAttachmentDto>();
            foreach (var file in files)
            {
                var response = await fileService.SaveFileAsync(file);
                results.Add(response);
            }
            return results;
        });
    }

    public async Task<CreateAttachmentDto> Upload(IFormFile file, IFileService fileService)
    {
        return await fileService.SaveFileAsync(file);
    }

    public async Task<List<CreateAttachmentDto>> UploadMultiple(IFormFileCollection files, IFileService fileService)
    {
        var results = new List<CreateAttachmentDto>();
        foreach (var file in files)
        {
            var response = await fileService.SaveFileAsync(file);
            results.Add(response);
        }
        return results;
    }
}
