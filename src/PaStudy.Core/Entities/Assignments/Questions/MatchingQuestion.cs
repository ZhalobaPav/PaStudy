namespace PaStudy.Core.Entities.Assignments.Questions;

public class MatchingQuestion: Question
{
    public virtual ICollection<MatchingPair> Pairs { get; set; } = new List<MatchingPair>();
}
