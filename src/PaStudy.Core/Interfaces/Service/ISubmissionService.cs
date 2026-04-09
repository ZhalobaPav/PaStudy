using PaStudy.Core.Helpers.DTOs.Submission;

namespace PaStudy.Core.Interfaces.Service;

public interface ISubmissionService
{
    Task CreateSubmission(CreateSubmissionDto submissionDto, string userId);
}
