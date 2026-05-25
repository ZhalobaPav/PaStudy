using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.Enums;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.DTOs.Assignment.Quiz;

public record CreateQuestionDto
{
    public string Text { get; set; } = string.Empty;
    public int Points { get; set; }
    public string? Feedback { get; set; } = string.Empty;
    public QuestionType Type { get; set; } = QuestionType.SingleChoice;
    public MatchingQuestionInfo? MatchingInfo { get; set; }
    public ChoiceQuestionInfo? ChoiceInfo { get; set; }
    public List<CreateAttachmentDto>? Attachments { get; set; }
}

public record StudentQuizDto(
    int Id,
    string Title,
    string? Description,
    int TimeLimitMinutes,
    DateTime? DueDate,
    int MaxPoints,
    ImmutableArray<StudentQuestionDto> Questions,
    ImmutableArray<AttachmentDto> Attachments
);

public record StudentQuestionDto(
    int Id,
    string Text,
    int Points,
    QuestionType Type,
    StudentChoiceInfo? ChoiceInfo,
    StudentMatchingInfo? MatchingInfo,
    List<AttachmentDto>? Attachments
);

public record AttemptResultDto(
    int AttemptId,
    int QuizId,
    decimal TotalScore,
    decimal MaxPoints,
    DateTimeOffset FinishedAt
);

public record AttemptAnswerPatchDto(
    int QuestionId,
    int? SelectedOptionId,
    List<int>? SelectedOptionIds,
    Dictionary<string, string>? MatchingAnswers,
    string? TextResponse
);

public record SavedAnswerDto(
    int QuestionId,
    int? SelectedOptionId,
    List<int>? SelectedOptionIds,
    Dictionary<string, string>? MatchingAnswers,
    string? TextResponse
);

public record StudentAnswerOption(int Id, string Text);
public record StudentChoiceInfo(List<StudentAnswerOption> Options);

public record StudentMatchingInfo(
    List<StudentAnswerOption> LeftSide,
    List<StudentAnswerOption> RightSide);
public record CreateAnswerOption(string Text, bool IsCorrect);
public record CreateMatchingPair(string LeftSide, string RightSide);

public record ChoiceQuestionInfo(List<CreateAnswerOption> Options);
public record MatchingQuestionInfo(List<CreateMatchingPair> Pairs);