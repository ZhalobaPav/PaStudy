using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;

namespace PaStudy.Infrastructure.Repositories;

public class AssignmentRepository: IAssignmentRepository
{
    private readonly PaStudyDbContext _dbContext;

    public AssignmentRepository(PaStudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Assignment> CreateAsync(Assignment assignment, CancellationToken ct = default)
    {
        var assignments = _dbContext.Set<Assignment>();
        await assignments.AddAsync(assignment, ct);
        await _dbContext.SaveChangesAsync(ct);
        return assignment;
    }
}
