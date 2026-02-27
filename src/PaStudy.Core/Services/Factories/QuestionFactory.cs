using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Interfaces.Factories;

namespace PaStudy.Core.Services.Factories;

public class QuestionFactory: IQuestionFactory
{
    private readonly IAttachmentFactory _attachmentFactory;

    public QuestionFactory(IAttachmentFactory attachmentFactory)
    {
        _attachmentFactory = attachmentFactory;
    }
    public Question CreateQuestion(CreateQuestionDto dto)
    {
        Question question = dto.Type switch
        {
            QuestionType.SingleChoice or QuestionType.MultipleChoice => CreateChoiceQuestion(dto),
            QuestionType.Matching => CreateMatchingQuestion(dto),
            _ => throw new ArgumentException($"Тип питання {dto.Type} не підтримується")
        };
        question.Text = dto.Text;
        question.Feedback = dto.Feedback;
        question.Points = dto.Points;
        question.Type = dto.Type;

        if (dto.Attachments?.Any() == true)
        {
            question.Attachments = dto.Attachments
                .Select(att => _attachmentFactory.CreateAttachment(att))
                .ToList();
        }
        return question;
    }

    private ChoiceQuestion CreateChoiceQuestion(CreateQuestionDto createQuestionDto)
    {
        return new ChoiceQuestion() 
        {
            Options = createQuestionDto.ChoiceInfo.Options.Select(opt => new AwnserOption
            {
                Text = opt.Text,
                IsCorrect = opt.IsCorrect
            }).ToList()
        };
    }

    private MatchingQuestion CreateMatchingQuestion(CreateQuestionDto dto)
    {
        if (dto.MatchingInfo == null)
            throw new ArgumentException("MatchingInfo обов'язковий для цього типу питання");
        return new MatchingQuestion()
        {
            Pairs = dto.MatchingInfo.Pairs.Select(opt => new MatchingPair
            {
                RightSide = opt.RightSide,
                LeftSide = opt.LeftSide,
            }).ToList(),
        };
    }
}
