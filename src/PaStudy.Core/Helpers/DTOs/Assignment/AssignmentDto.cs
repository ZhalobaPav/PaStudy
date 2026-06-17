using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.Enums;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.DTOs.Assignment;

public class AssignmentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime? StartDate { get; set; }
    public ImmutableArray<AttachmentDto>? Attachments { get; set; } = new ImmutableArray<AttachmentDto>();
    public int MaxPoints { get; set; } = 100;
    public AssignmentType AssignmentType { get; set; } = AssignmentType.Task;
    public QuizInfoBrief? QuizInfo { get; set; }
    public SubmissionInfo? SubmissionInfo { get; set; }
    public AssignmentStatus? Status { get; set; }
}
public enum AssignmentStatus
{
    NotStarted = 0,
    Submited = 1,
    Graded = 2
}

public record struct QuizInfoBrief(bool ShuffleQuestions, int TimeLimitMinutes, int questionQuantity);

public record SubmissionInfo(
    bool IsSubmitted,
    TaskSubmissionDto? TaskSubmission,
    QuizSubmissionInfoDto? quizSubmission,
    DateTimeOffset? SubmittedAt,
    decimal? Grade,
    string? TeacherFeedback
);
public class BulkCreateSectionDto
{
    public int CourseId { get; set; }
    public List<SectionItemDto> Sections { get; set; } = new();
}

public class SectionItemDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
public record TaskSubmissionDto(string studentNote, ImmutableArray<AttachmentDto>? Attachments);
public record QuizSubmissionInfoDto(decimal TotalScore, QuizAttemptStatus attemptStatus, DateTimeOffset FinishedAt);