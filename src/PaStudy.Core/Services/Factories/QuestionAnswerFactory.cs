using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Interfaces.Factories;

namespace PaStudy.Core.Services.Factories;

public class QuestionAnswerFactory: IQuestionAnswerFactory
{
    public QuestionAnswer CreateAnswer(Question question, object rawData)
    {
        return question.Type switch
        {
            QuestionType.SingleChoice or QuestionType.MultipleChoice =>
                CreateChoiceAnswer(question.Id, rawData),

            QuestionType.Matching =>
                CreateMatchingAnswer(question.Id, rawData),

            _ => throw new NotSupportedException($"Question type {question.Type} cannot be used yet")
        };
    }
    private ChoiceAnswer CreateChoiceAnswer(int questionId, object rawData)
    {
        if (rawData is not int optionId)
            throw new ArgumentException("for choice type need to be int (OptionId)");

        return new ChoiceAnswer
        {
            QuestionId = questionId,
            SelectedOptionId = optionId
        };
    }

    private MatchingAnswer CreateMatchingAnswer(int questionId, object rawData)
    {
        if (rawData is not Dictionary<int, string> selectedPairs)
            throw new ArgumentException("For MatchingAnswer data should be Dictionary<int, string>");

        return new MatchingAnswer
        {
            QuestionId = questionId,
            SelectedPairs = selectedPairs.Select(p => new MatchingAnswerPair
            {
                MatchingPairId = p.Key,
                SelectedRightSideValue = p.Value
            }).ToList()
        };
    }
}
