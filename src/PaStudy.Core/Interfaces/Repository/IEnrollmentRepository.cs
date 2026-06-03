using PaStudy.Core.Helpers.DTOs.Enrollment;
using System.Security.Claims;

namespace PaStudy.Core.Interfaces.Repository;

public interface IEnrollmentRepository
{
    Task<bool> CreateEnrollmentAsync(int courseId, ClaimsPrincipal user, CancellationToken cancellationToken);
    Task<List<TeacherGradebookDto>> GetCourseGradebookAsync(int courseId, CancellationToken cancellationToken);
}
