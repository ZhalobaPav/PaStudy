using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities.Assignments.Questions;

public class MatchingPair: BaseEntity
{
    public string LeftSide { get; set; } = string.Empty;
    public string RightSide { get; set; } = string.Empty;
    public int QuestionId { get; set; }
}
