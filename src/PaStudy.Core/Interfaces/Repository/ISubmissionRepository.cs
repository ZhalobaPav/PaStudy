using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Helpers.DTOs.Submission;
using PaStudy.Core.Helpers.FilterObjects.Submissions;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PaStudy.Core.Interfaces.Repository;

public interface ISubmissionRepository
{
    Task CreateSubmission(Submission submission);
    Task<ImmutableArray<SubmissionListItemDto>> GetSubmissionsByAssignmentIdAsync(SubmissionFilter filter, CancellationToken cancellationToken);
    Task<TaskSubmissionDto> GetSubmissionByIdAsync(int id, ClaimsPrincipal user);
    Task<TaskSubmissionDto> GradeSubmissionAsync(GradeSubmissionDto dto, ClaimsPrincipal user);
    Task UpdateCourseProgress(int studentId, int courseId);
}
