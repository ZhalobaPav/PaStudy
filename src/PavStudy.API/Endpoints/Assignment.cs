using Microsoft.AspNetCore.Mvc;
using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.DTOs.Reponses;
using PaStudy.Core.Helpers.DTOs.Section;
using PaStudy.Core.Helpers.Exceptions;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PavStudy.API.Endpoints;

public class Assignment: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).RequireAuthorization()
            .MapGet(GetAssignments, "sections/{courseId}")
            .MapGet(GetAssignmentById, "/{assignmentId}")
            .MapPost(CreateAssignment)
            .MapPost(CreateSectionAsync, "section")
            .MapPost(StartAttemptAsync, "quiz/{quizId}/startAttempt")
            .MapPatch(SaveAnswer, "attempts/{attemptId}")
            .MapPost(SubmitAnswers, "attempts/{attemptId}/submit")
            .MapDelete(DeleteAssignment, "/{id}");
    }

    public async Task<BaseResponse<PaStudy.Core.Entities.Assignments.Assignment>> CreateAssignment(CreateAssignmentDto dto, IAssignmentService assignmentService, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return await assignmentService.CreateAssignmentAsync(dto, userId);
    }

    public async Task<BaseResponse<Section>> CreateSectionAsync(CreateSectionDto dto, IAssignmentService assignmentService, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return await assignmentService.CreateSectionAsync(dto, userId);
    }

    public async Task<ImmutableArray<SectionDto>> GetAssignments(int courseId, IAssignmentRepository assignmentRepository, CancellationToken cancellationToken, ClaimsPrincipal user)
    {
        return await assignmentRepository.GetSectionsAsync(courseId, cancellationToken, user);
    }

    public async Task<AssignmentDto> GetAssignmentById(int assignmentId, CancellationToken cancellationToken, IAssignmentRepository assignmentRepository, ClaimsPrincipal user)
    {
        return await assignmentRepository.GetAssignmentByIdAsync(assignmentId, cancellationToken, user);
    }

    public async Task<IResult> StartAttemptAsync(int quizId, ClaimsPrincipal user, IQuizRepository quizRepository)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) 
        {
            throw new UnauthorizedAccessException("You do not have access");
        }

        var result = await quizRepository.StartAttemptAsync(quizId, userId);
        return Results.Ok(result);
    }

    public async Task<IResult> SaveAnswer(int attemptId, [FromBody] AttemptAnswerPatchDto request, ClaimsPrincipal user, IQuizRepository quizRepository)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }

        await quizRepository.SaveAnswerAsync(attemptId, userId, request);

        return Results.Ok();
    }

    public async Task<IResult> SubmitAnswers(int attemptId, ClaimsPrincipal user, IQuizRepository quizRepository)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }

        var result = await quizRepository.SubmitAttemptAsync(attemptId, userId);

        return Results.Ok(result);
    }

    public async Task<IResult> DeleteAssignment(int id, CancellationToken ct, IAssignmentRepository assignmentRepository, ClaimsPrincipal user)
    {
        await assignmentRepository.DeleteAssignmentAsync(id, user);
        return Results.Ok(new { message = "Завдання успішно видалено" });
    }

}
