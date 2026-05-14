using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Infrastructure.Data;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using System.Collections.Immutable;
using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Core.Helpers.Enums;

namespace PaStudy.Infrastructure.Repositories;

public class QuizRepository: IQuizRepository
{
    private readonly PaStudyDbContext _dbContext;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IStudentRepository _studentRepository;

    public QuizRepository(PaStudyDbContext dbContext, ISubmissionRepository submissionRepository, IStudentRepository studentRepository)
    {
        _dbContext = dbContext;
        _submissionRepository = submissionRepository;
        _studentRepository = studentRepository;
    }
    public async Task<AttemptStartResponseDto> StartAttemptAsync(int quizId, string userId)
    {
        var activeAttempt = await _dbContext.Set<QuizAttempt>()
            .Include(a => a.Answers)
            .Include(a => a.Quiz)
                .ThenInclude(q => q.Questions)
                    .ThenInclude(q => (q as ChoiceQuestion).Options)
            .Include(a => a.Quiz)
                .ThenInclude(q => q.Questions)
                    .ThenInclude(q => (q as MatchingQuestion).Pairs)
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
    public async Task<AttemptResultDto> SubmitAttemptAsync(int attemptId, string userId)
    {
        var attempt = await _dbContext.Set<QuizAttempt>()
            .Include(a => a.Answers)
            .Include(a => a.Quiz)
                .ThenInclude(q => q.Questions)
                    .ThenInclude(q => (q as ChoiceQuestion).Options)
            .Include(a => a.Quiz)
                .ThenInclude(q => q.Questions)
                    .ThenInclude(q => (q as MatchingQuestion).Pairs)
            .Include(a => a.Quiz)
                .ThenInclude(q => q.Section)
            .FirstOrDefaultAsync(a => a.Id == attemptId && a.UserId == userId);
        
        if (attempt == null)
            throw new KeyNotFoundException("Спробу не знайдено або ви не маєте до неї доступу.");

        if (attempt.Status != QuizAttemptStatus.InProgress)
            throw new InvalidOperationException("Ця спроба вже завершена.");
        var student = await _studentRepository.GetByUserIdAsync(userId);
        if (student == null)
            throw new ArgumentException("Student not found for the given user ID.");

        decimal totalAttemptScore = 0;
        decimal maxAttemptPoints = 0;

        foreach (var question in attempt.Quiz.Questions)
        {
            maxAttemptPoints += question.Points;

            var studentAnswer = attempt.Answers.FirstOrDefault(a => a.QuestionId == question.Id);
            if (studentAnswer == null) continue;

            decimal pointsEarned = 0;
            bool isCorrect = false;

            if (question is ChoiceQuestion cq)
            {
                //if (question.Type == QuestionType.SingleChoice)
                //{
                //    var correctOption = cq.Options.FirstOrDefault(o => o.IsCorrect);
                //    if (correctOption != null && studentAnswer.SelectedOptionId == correctOption.Id)
                //    {
                //        pointsEarned = question.Points;
                //        isCorrect = true;
                //    }
                //}
                if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.SingleChoice)
                {
                    var correctOptionIds = cq.Options.Where(o => o.IsCorrect).Select(o => o.Id).ToList();
                    var userOptionIds = studentAnswer.SelectedOptionIds ?? new List<int>();

                    if (correctOptionIds.Count == userOptionIds.Count && !correctOptionIds.Except(userOptionIds).Any())
                    {
                        pointsEarned = question.Points;
                        isCorrect = true;
                    }
                }
            }
            else if (question is MatchingQuestion mq)
            {
                if (studentAnswer.MatchingAnswers != null && studentAnswer.MatchingAnswers.Count > 0)
                {
                    int correctPairsCount = 0;
                    var totalPairs = mq.Pairs.Count;

                    foreach (var pair in mq.Pairs)
                    {
                        if (studentAnswer.MatchingAnswers.TryGetValue(pair.Id.ToString(), out string? rightIdStr) &&
                            int.TryParse(rightIdStr, out int rightId) &&
                            rightId == pair.Id)
                        {
                            correctPairsCount++;
                        }
                    }

                    if (totalPairs > 0)
                    {
                        pointsEarned = question.Points * ((decimal)correctPairsCount / totalPairs);
                        isCorrect = correctPairsCount == totalPairs;
                    }
                }
            }

            studentAnswer.PointsEarned = pointsEarned;
            studentAnswer.IsCorrect = isCorrect;

            totalAttemptScore += pointsEarned;
        }

        attempt.Status = QuizAttemptStatus.Completed;
        attempt.SubmittedAt = DateTime.UtcNow;
        attempt.TotalScore = totalAttemptScore;

        await _dbContext.SaveChangesAsync();
        await _submissionRepository.UpdateCourseProgress(student.Id, attempt.Quiz.Section.CourseId);
        return new AttemptResultDto(
            AttemptId: attempt.Id,
            QuizId: attempt.QuizId,
            TotalScore: attempt.TotalScore.Value,
            MaxPoints: maxAttemptPoints,
            FinishedAt: attempt.SubmittedAt.Value
        );
    }
    public async Task SaveAnswerAsync(int attemptId, string userId, AttemptAnswerPatchDto dto)
    {
        var attempt = await _dbContext.Set<QuizAttempt>()
            .Include(a => a.Quiz)
            .Include(a => a.Answers)
            .FirstOrDefaultAsync(a => a.Id == attemptId && a.UserId == userId);

        if (attempt == null)
        {
            throw new KeyNotFoundException("Спробу не знайдено або ви не маєте до неї доступу.");
        }

        if (attempt.Status != QuizAttemptStatus.InProgress)
        {
            throw new InvalidOperationException("Неможливо зберегти відповідь: спроба вже завершена.");
        }

        if (attempt.IsExpired)
        {
            attempt.Status = QuizAttemptStatus.Expired;
            await _dbContext.SaveChangesAsync();
            throw new InvalidOperationException("Час на виконання квіза вичерпано.");
        }

        var existingAnswer = attempt.Answers.FirstOrDefault(a => a.QuestionId == dto.QuestionId);

        if (existingAnswer == null)
        {
            existingAnswer = new QuizAttemptAnswer
            {
                QuizAttemptId = attemptId,
                QuestionId = dto.QuestionId,
                SelectedOptionId = dto.SelectedOptionId,
                SelectedOptionIds = dto.SelectedOptionIds,
                MatchingAnswers = dto.MatchingAnswers,
                TextResponse = dto.TextResponse
            };
            _dbContext.Set<QuizAttemptAnswer>().Add(existingAnswer);
        }
        else
        {
            existingAnswer.SelectedOptionId = dto.SelectedOptionId;
            existingAnswer.SelectedOptionIds = dto.SelectedOptionIds;
            existingAnswer.MatchingAnswers = dto.MatchingAnswers;
            existingAnswer.TextResponse = dto.TextResponse;
        }

        await _dbContext.SaveChangesAsync();
    }
}
