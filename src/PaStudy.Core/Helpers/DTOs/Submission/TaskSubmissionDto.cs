using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.Enums;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.DTOs.Submission;

public record GradeSubmissionDto
{
    public int SubmissionId { get; set; }
    public decimal Grade { get; set; }
    public string? TeacherFeedback { get; set; }
}
public record TaskSubmissionDto
{
    public int Id { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string? StudentNotes { get; set; }

    public List<AttachmentDto> Attachments { get; set; } = new();

    public decimal? Grade { get; set; }
    public string? TeacherFeedback { get; set; }
    public SubmissionStatus Status { get; set; }
    public StudentInfo StudentInfo { get; set; }
    public AssignmentInfo AssignmentInfo { get; set; }
    public SubmissionQuizInfoDto? QuizInfo = null;
}
public record StudentInfo(string studentFullName, string? studentEmail);

public record AssignmentInfo (string Title, string Description, DateTime? DueDate, int MaxPoints);
public record SubmissionListItemDto
{
    public int Id { get; set; }
    public int? StudentId { get; set; }
    public string StudentFullName { get; set; }
    public string? StudentEmail { get; set; }
    public DateTimeOffset SubmittedAt { get; set; }
    public decimal? Grade { get; set; }
    public SubmissionStatus Status { get; set; } = SubmissionStatus.Draft;
}

public record SubmissionQuizInfoDto
{
    public ImmutableArray<SavedAnswerDto> SavedAnswers { get; set; } = ImmutableArray<SavedAnswerDto>.Empty;
    public int TimeLimitMinutes { get; set; }
}