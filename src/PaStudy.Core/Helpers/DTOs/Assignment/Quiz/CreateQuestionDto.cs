using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.Enums;

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

public record CreateAnswerOption(string Text, bool IsCorrect);
public record CreateMatchingPair(string LeftSide, string RightSide);

public record ChoiceQuestionInfo(List<CreateAnswerOption> Options);
public record MatchingQuestionInfo(List<CreateMatchingPair> Pairs);