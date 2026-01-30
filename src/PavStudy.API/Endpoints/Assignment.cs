using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Assignment;
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
            .MapGet(GetAssignments, "{courseId}")
            .MapPost(CreateAssignment)
            .MapPost(CreateSectionAsync, "section");
    }

    public async Task<BaseResponse<PaStudy.Core.Entities.Assignment>> CreateAssignment(CreateAssignmentDto dto, IAssignmentService assignmentService, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return await assignmentService.CreateAssignmentAsync(dto, userId);
    }

    public async Task<BaseResponse<Section>> CreateSectionAsync(CreateSectionDto dto, IAssignmentService assignmentService, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var allClaims = user.Claims.Select(c => $"{c.Type}: {c.Value}");
        return await assignmentService.CreateSectionAsync(dto, userId);
    }

    public async Task<ImmutableArray<SectionDto>> GetAssignments(int courseId, IAssignmentRepository assignmentRepository, CancellationToken cancellationToken)
    {
        return await assignmentRepository.GetSectionsAsync(courseId, cancellationToken);
    }
}
