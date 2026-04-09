using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Helpers.DTOs.Submission;

namespace PaStudy.Core.Interfaces.Service;

public interface IQuizSubmissionService
{
    Task<QuizSubmission> ProcessSubmissionAsync(CreateSubmissionDto dto);
}
