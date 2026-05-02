using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Infrastructure.Data;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using System.Collections.Immutable;
using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Interfaces.Repository;

namespace PaStudy.Infrastructure.Repositories;

public class QuizRepository: IQuizRepository
{
    private readonly PaStudyDbContext _dbContext;

    public QuizRepository(PaStudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<AttemptStartResponseDto> StartAttemptAsync(int quizId, string userId)
    {
        var activeAttempt = await _dbContext.Set<QuizAttempt>()
            .Include(a => a.Answers)
            .Include(a => a.Quiz)
                .ThenInclude(q => q.Questions)
                    .ThenInclude(q => (q as ChoiceQuestion).Options)
            .Include(a => a.Quiz)
                .ThenInclude(q => q.Attachments)
            .FirstOrDefaultAsync(a => a.QuizId == quizId
                                 && a.UserId == userId
                                 && a.Status == QuizAttemptStatus.InProgress);
        if (activeAttempt != null)
        {
            var studentQuestions = activeAttempt.Quiz.Questions
                .Select(q => q.ToStudentQuestionDto())
                .ToImmutableArray();

            var attachments = activeAttempt.Quiz.Attachments
                .Select(a => new AttachmentDto
                {
                    FileName = a.FileName,
                    FileUrl = a.FileUrl
                })
                .ToImmutableArray();
            var savedAnswers = activeAttempt.Answers
                .Select(ans => new SavedAnswerDto(
                    ans.QuestionId,
                    ans.SelectedOptionId,
                    ans.SelectedOptionIds,
                    ans.MatchingAnswers,
                    ans.TextResponse
                ))
                .ToImmutableArray();

            return activeAttempt.ToAttemptResponseDto(
                activeAttempt.Quiz,
                studentQuestions,
                attachments,
                savedAnswers
            );
        }
        var quiz = await _dbContext.Set<QuizAssignment>()
            .Include(q => q.Questions)
                .ThenInclude(q => (q as ChoiceQuestion).Options)
            .Include(q => q.Attachments)
            .FirstOrDefaultAsync(q => q.Id == quizId);

        if (quiz == null)
        {
            throw new KeyNotFoundException($"Квіз із ID {quizId} не знайдено");
        }

        var newAttempt = new QuizAttempt
        {
            QuizId = quizId,
            UserId = userId,
            StartedAt = DateTime.UtcNow,
            Status = QuizAttemptStatus.InProgress
        };

        _dbContext.Set<QuizAttempt>().Add(newAttempt);
        await _dbContext.SaveChangesAsync();

        var newStudentQuestions = quiz.Questions
        .Select(q => q.ToStudentQuestionDto())
        .ToImmutableArray();

        var newAttachments = quiz.Attachments
            .Select(a => new AttachmentDto
            {
                FileName = a.FileName,
                FileUrl = a.FileUrl
            })
            .ToImmutableArray();

        return newAttempt.ToAttemptResponseDto(
            quiz,
            newStudentQuestions,
            newAttachments,
            ImmutableArray<SavedAnswerDto>.Empty
        );

    }
}
