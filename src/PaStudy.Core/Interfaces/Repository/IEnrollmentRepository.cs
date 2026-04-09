using System.Security.Claims;

namespace PaStudy.Core.Interfaces.Repository;

public interface IEnrollmentRepository
{
    Task<bool> CreateEnrollmentAsync(int courseId, ClaimsPrincipal user, CancellationToken cancellationToken);
}
