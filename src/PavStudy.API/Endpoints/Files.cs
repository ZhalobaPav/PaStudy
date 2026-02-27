using PaStudy.Core.Interfaces.Service;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;

namespace PavStudy.API.Endpoints;

public class Files : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).RequireAuthorization()
            .MapPost("upload", Upload).DisableAntiforgery();
    }

    public async Task<string> Upload(IFormFile file, IFileService fileService)
    {
        return await fileService.SaveFileAsync(file);
    }
}
