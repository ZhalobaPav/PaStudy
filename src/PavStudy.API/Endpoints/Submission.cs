using Microsoft.AspNetCore.Mvc;
using PaStudy.Core.Helpers.DTOs.Submission;
using PaStudy.Core.Helpers.Exceptions;
using PaStudy.Core.Helpers.FilterObjects.Submissions;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PavStudy.API.Endpoints;

public class Submission : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).RequireAuthorization()
            .MapGet(GetSubmissionsByAssignmentId)
            .MapGet(GetSubmissionById, "/{id}")
            .MapGet(GetStudentAwnsers, "quizAttempt/{attemptId}")
            .MapPost(SubmitAssignment)
            .MapPost(SubmitAssignmentDev, "dev-submit/{userId}")
            .MapPut(GradeSubmissionTask);
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
    public async Task<IResult> GetSubmissionById(int id, ISubmissionRepository submissionService, ClaimsPrincipal user)
    {
        try
        {
            var submission = await submissionService.GetSubmissionByIdAsync(id, user);
            if (submission == null)
                return Results.NotFound();
            return Results.Ok(submission);
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
    public async Task<IResult> GetSubmissionsByAssignmentId([AsParameters] SubmissionFilter filter, ISubmissionRepository submissionService, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();
        try
        {
            var submissions = await submissionService.GetSubmissionsByAssignmentIdAsync(filter, CancellationToken.None);
            return Results.Ok(submissions);
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

    public async Task<IResult> GetStudentAwnsers(int attemptId, ClaimsPrincipal user, IQuizRepository quizRepository)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            throw new UnauthorizedAccessException("You do not have access");
        }
        if (!user.IsInRole("Teacher"))
        {
            throw new ForbiddenException("You do not have access");
        }
        try
        {
            var result = await quizRepository.GetQuizAttemptDetailsAsync(attemptId);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }

    }
    public async Task<IResult> GradeSubmissionTask(GradeSubmissionDto dto, ISubmissionRepository submissionRepository, ClaimsPrincipal user)
    {
        try
        {
            var submission = await submissionRepository.GradeSubmissionAsync(dto, user);

            return Results.Ok(submission);
        }
        catch (ArgumentException ex)
        {
            return Results.NotFound(new { error = ex.Message });
        }
        catch (BadRequestException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    public static async Task<IResult> SubmitAssignmentDev(
    [FromBody] CreateSubmissionDto dto,
    [FromRoute] string userId, // Передаємо в Postman через URL: ?userId=твій-guid
    ISubmissionService submissionService)
    {
        if (string.IsNullOrEmpty(userId))
            return Results.BadRequest("User ID is required for dev endpoint.");

        try
        {
            await submissionService.CreateSubmission(dto, userId);
            return Results.Ok(new { message = "Dev Submission created successfully" });
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

