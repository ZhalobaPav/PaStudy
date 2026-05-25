using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.DTOs.Attachment;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.Extensions.MapperHelpers;

public static class QuizMapper
{
    public static AttemptStartResponseDto ToAttemptResponseDto(
        this QuizAttempt quizAttempt,
        QuizAssignment quiz,
        ImmutableArray<StudentQuestionDto> questions,
        ImmutableArray<AttachmentDto> attachments,
        ImmutableArray<SavedAnswerDto> savedAnswers)
    {
        return new AttemptStartResponseDto
        {
            AttemptId = quizAttempt.Id,
            QuizId = quizAttempt.QuizId,
            Title = quiz.Title,
            Description = quiz.Description,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            StartedAt = quizAttempt.StartedAt,
            DueDate = quiz.DueDate,
            MaxPoints = quiz.MaxPoints,
            Questions = questions,
            Attachments = attachments,
            SavedAnswers = savedAnswers,
            TotalScore = quizAttempt.TotalScore
        };
    }

    public static StudentQuestionDto ToStudentQuestionDto(this Question question)
    {
        StudentChoiceInfo? choiceInfo = null;
        StudentMatchingInfo? matchingInfo = null;

        if (question is ChoiceQuestion cq)
        {
            choiceInfo = new StudentChoiceInfo(
                cq.Options.Select(o => new StudentAnswerOption(o.Id, o.Text))
                          .OrderBy(_ => Guid.NewGuid())
                          .ToList()
            );
        }
        else if (question is MatchingQuestion mq)
        {
            matchingInfo = new StudentMatchingInfo(
                mq.Pairs.Select(p => new StudentAnswerOption(p.Id, p.LeftSide))
                        .OrderBy(_ => Guid.NewGuid()).ToList(),

                mq.Pairs.Select(p => new StudentAnswerOption(p.Id, p.RightSide))
                        .OrderBy(_ => Guid.NewGuid()).ToList()
            );
        }

        var attachments = question.Attachments?
            .Select(a => new AttachmentDto
            {
                FileName = a.FileName,
                FileUrl = a.FileUrl,
                ContentType = a.ContentType
            })
            .ToList();

        return new StudentQuestionDto(
            question.Id,
            question.Text,
            question.Points,
            question.Type,
            choiceInfo,
            matchingInfo,
            attachments
        );
    }
}
