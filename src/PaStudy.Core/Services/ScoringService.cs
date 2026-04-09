using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Interfaces.Service;

namespace PaStudy.Core.Services;

public class ScoringService: IScoringService
{
    public decimal CalculateScore(Question question, QuestionAnswer answer)
    {
        return (question, answer) switch
        {
            (ChoiceQuestion q, ChoiceAnswer a) => CalculateChoice(q, a),
            (MatchingQuestion q, MatchingAnswer a) => CalculateMatching(q, a),
            _ => throw new NotSupportedException($"Scoring for question type {question.Type} is not supported yet")
        };
    }
    private decimal CalculateChoice(ChoiceQuestion? q, ChoiceAnswer a)
    {
        if (q == null) return 0;
        var correctOption = q.Options.FirstOrDefault(o => o.IsCorrect);
        return a.SelectedOptionId == correctOption?.Id ? q.Points : 0;
    }

    private decimal CalculateMatching(MatchingQuestion? q, MatchingAnswer a)
    {
        if (q == null || !q.Pairs.Any()) return 0;

        int correctCount = 0;
        foreach (var userPair in a.SelectedPairs)
        {
            var originalPair = q.Pairs.FirstOrDefault(p => p.Id == userPair.MatchingPairId);
            if (originalPair != null && originalPair.RightSide == userPair.SelectedRightSideValue)
            {
                correctCount++;
            }
        }
        return (decimal)correctCount / q.Pairs.Count * q.Points;
    }
}
