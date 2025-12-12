using PaStudy.Core.Helpers.DTOs.Group;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Collections.Immutable;

namespace PavStudy.API.Endpoints;

public class Groups : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetGroups);
    }
    public async Task<ImmutableArray<GroupDto>> GetGroups(CancellationToken cancellationToken, IGroupRepository groupRepository)
    {
        return await groupRepository.GetGroupsAsync(cancellationToken);
    }
}
