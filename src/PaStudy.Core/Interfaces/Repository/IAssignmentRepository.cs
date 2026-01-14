using PaStudy.Core.Entities;

namespace PaStudy.Core.Interfaces.Repository;

public interface IAssignmentRepository
{
    Task<Assignment> CreateAsync(Assignment assignment, CancellationToken ct = default);
}
