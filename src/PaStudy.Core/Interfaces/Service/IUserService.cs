using PaStudy.Core.Helpers.DTOs.Users;
using PaStudy.Core.Helpers.FilterObjects.UserFilters;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Service;

public interface IUserService
{
    Task<ImmutableArray<UserProfileResponseDto>> GetUsers(UserFilter userFilter, CancellationToken cancellationToken);
}
