using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Group;
using PaStudy.Core.Helpers.DTOs.Teacher;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using System.Collections.Immutable;

namespace PaStudy.Infrastructure.Repositories;

public class GroupRepository: IGroupRepository
{
    private readonly PaStudyDbContext _context;

    public GroupRepository(PaStudyDbContext context)
    {
        _context = context;
    }

    public async Task<ImmutableArray<GroupDto>> GetGroupsAsync(CancellationToken cancellationToken)
    {
        var groups = _context.Set<Group>();

        return await groups.Select(g => new GroupDto
        {
            Id = g.Id,
            GroupNumber = g.GroupNumber,
            InstitutionNumber = g.InstitutionNumber,
            Year = g.Year,
            Faculty = g.Faculty,
            Speciality = g.Speciality,
            Teacher = new TeacherDto
            {
                Id = g.CuratorOfGroup.Id,
                FirstName = g.CuratorOfGroup.FirstName,
                LastName = g.CuratorOfGroup.LastName,
                MiddleName = g.CuratorOfGroup.MiddleName
            }
        }).ToImmutableArrayAsync(cancellationToken);

    }

}
