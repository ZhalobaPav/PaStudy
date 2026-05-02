using PaStudy.Core.Helpers.DTOs.Attachment;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.DTOs.Assignment.Quiz;

public record AttemptStartResponseDto {
    public int AttemptId { get; set; }
    public int QuizId { get; set; }
    public string Title {  get; set; }
    public string? Description { get; set; }
    public int TimeLimitMinutes { get; set; }
    public DateTime StartedAt {  get; set; }
    public DateTime? DueDate { get; set; }
    public int MaxPoints { get; set; }
    public ImmutableArray<StudentQuestionDto> Questions { get; set; }
    public ImmutableArray<AttachmentDto> Attachments { get; set; }
    public ImmutableArray<SavedAnswerDto> SavedAnswers { get; set; } = ImmutableArray<SavedAnswerDto>.Empty;
}