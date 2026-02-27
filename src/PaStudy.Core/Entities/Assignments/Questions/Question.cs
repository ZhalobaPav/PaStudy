using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Entities.Base;
using PaStudy.Core.Helpers.Enums;

namespace PaStudy.Core.Entities.Assignments.Questions;

public abstract class Question : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public string? Feedback { get; set; } = string.Empty;
    public int Points { get; set; }
    public QuestionType Type { get; set; } = QuestionType.SingleChoice;
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
