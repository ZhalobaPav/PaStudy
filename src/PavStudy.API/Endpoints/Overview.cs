using PaStudy.Core.Helpers.DTOs.Profile;
using PaStudy.Core.Helpers.FilterObjects.CourseFilters;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Security.Claims;

namespace PavStudy.API.Endpoints;

public class Overview : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).RequireAuthorization().MapGet(GetProfileInfo);
    }

    public async Task<IResult> GetProfileInfo(CancellationToken cancellationToken, ClaimsPrincipal user, IOverviewService service, [AsParameters] CourseFilter courseFilter)
    {
        try
        {
            var result = await service.GetProfileInfo(courseFilter, user, cancellationToken);
            return Results.Ok(result);
        }
        catch (Exception ex) 
        {
            return Results.Problem(ex.Message, ex.StackTrace);
        }
    }
}
