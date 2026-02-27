using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities.Assignments.Questions;

public class AwnserOption : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
