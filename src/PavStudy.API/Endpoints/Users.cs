using Microsoft.AspNetCore.Mvc;
using PaStudy.Core.Helpers.DTOs.Users;
using PaStudy.Core.Helpers.FilterObjects.UserFilters;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Collections.Immutable;

namespace PavStudy.API.Endpoints
{
    public class Users : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .MapGet(GetUsers);
        }
        public async Task<ImmutableArray<UserProfileResponseDto>> GetUsers([AsParameters] UserFilter filter,CancellationToken cancellationToken, IUserService userService)
        {
            return await userService.GetUsers(filter, cancellationToken);
        }
    }
}
