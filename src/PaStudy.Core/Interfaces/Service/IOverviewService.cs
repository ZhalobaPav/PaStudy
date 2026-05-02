using PaStudy.Core.Helpers.DTOs.Profile;
using PaStudy.Core.Helpers.FilterObjects.CourseFilters;
using System.Security.Claims;

namespace PaStudy.Core.Interfaces.Service;

public interface IOverviewService
{
    Task<BaseProfileDto> GetProfileInfo(CourseFilter courseFilter, ClaimsPrincipal user, CancellationToken cancellationToken);
}
