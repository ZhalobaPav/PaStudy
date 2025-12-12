using PaStudy.Core.Helpers.DTOs.Group;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Repository;

public interface IGroupRepository
{
    Task<ImmutableArray<GroupDto>> GetGroupsAsync(CancellationToken cancellationToken);
}
