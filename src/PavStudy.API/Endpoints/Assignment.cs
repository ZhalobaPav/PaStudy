using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Reponses;
using PaStudy.Core.Helpers.Exceptions;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Security.Claims;

namespace PavStudy.API.Endpoints;

public class Assignment: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreateAssignment);
    }

    public async Task<BaseResponse<PaStudy.Core.Entities.Assignment>> CreateAssignment(CreateAssignmentDto dto, IAssignmentService assignmentService, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return await assignmentService.CreateAssignmentAsync(dto, userId);
    }
}
