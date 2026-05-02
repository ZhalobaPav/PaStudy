using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;

namespace PaStudy.Core.Interfaces.Repository;

public interface IQuizRepository
{
    Task<AttemptStartResponseDto> StartAttemptAsync(int quizId, string userId);
}
