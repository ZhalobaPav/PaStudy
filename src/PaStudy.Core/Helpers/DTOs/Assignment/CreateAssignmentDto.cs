using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.Enums;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.DTOs.Assignment;

public class CreateAssignmentDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ImmutableArray<CreateAttachmentDto> Attachments { get; set; } = ImmutableArray<CreateAttachmentDto>.Empty;
    public AssignmentType AssignmentType { get; set; } = AssignmentType.Task;
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int MaxPoints { get; set; } = 100;
    public int SectionId { get; set; }
    public QuizInfo? QuizInfo { get; set; }
}

public record QuizInfo (ICollection<CreateQuestionDto> Questions, bool ShuffleQuestions, int TimeLimitMinutes);