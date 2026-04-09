using PaStudy.Core.Helpers.DTOs.Submission;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Security.Claims;

namespace PavStudy.API.Endpoints;

public class Submission : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).RequireAuthorization().MapPost(SubmitAssignment);
    }

    public async Task<IResult> SubmitAssignment(CreateSubmissionDto dto, ISubmissionService submissionService, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();
        try
        {
            await submissionService.CreateSubmission(dto, userId);
            return Results.Ok(new { message = "Submission created successfully" });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Results.StatusCode(500);
        }
    }
}
