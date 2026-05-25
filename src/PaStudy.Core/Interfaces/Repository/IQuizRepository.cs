using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;

namespace PaStudy.Core.Interfaces.Repository;

public interface IQuizRepository
{
    Task<AttemptStartResponseDto> StartAttemptAsync(int quizId, string userId);
    Task<AttemptResultDto> SubmitAttemptAsync(int attemptId, string userId);
    Task SaveAnswerAsync(int attemptId, string userId, AttemptAnswerPatchDto dto);
    Task<AttemptStartResponseDto> GetQuizAttemptDetailsAsync(int attemptId);
}
