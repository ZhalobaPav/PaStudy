using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities.Assignments.Submission;

public abstract class QuestionAnswer: BaseEntity
{
    public int QuestionId { get; set; }
    public int QuizSubmissionId { get; set; }
    public decimal PointsAwarded { get; set; }
}

public class ChoiceAnswer : QuestionAnswer
{
    public int SelectedOptionId { get; set; }
}

public class MatchingAnswer : QuestionAnswer
{   
    public ICollection<MatchingAnswerPair> SelectedPairs { get; set; }
}

public class MatchingAnswerPair : BaseEntity
{
    public int MatchingAnswerId { get; set; }
    public MatchingAnswer MatchingAnswer { get; set; } = null!;

    public int MatchingPairId { get; set; }
    public MatchingPair MatchingPair { get; set; } = null!;
    public string SelectedRightSideValue { get; set; } = string.Empty;
}