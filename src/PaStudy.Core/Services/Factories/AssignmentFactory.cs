using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Interfaces.Factories;

namespace PaStudy.Core.Services.Factories;

public class AssignmentFactory : IAssignmentElementFactory
{
    private readonly IAttachmentFactory _attachmentFactory;
    private readonly IQuestionFactory _questionFactory;

    public AssignmentFactory(IAttachmentFactory attachmentFactory, IQuestionFactory questionFactory)
    {
        _attachmentFactory = attachmentFactory;
        _questionFactory = questionFactory;
    }
    public Assignment CreateAssignment(CreateAssignmentDto createAssignmentDto)
    {
        return createAssignmentDto.AssignmentType switch
        {
            AssignmentType.Task => CreateTaskAssignment(createAssignmentDto),
            AssignmentType.Quiz => CreateQuizAssignment((CreateQuizDto)createAssignmentDto),
            _ => throw new ArgumentException("Invalid assignment type")
        };
    }

    private QuizAssignment CreateQuizAssignment(CreateQuizDto quizDto)
    {
        return new QuizAssignment
        {
            Title = quizDto.Title,
            Description = quizDto.Description,
            ShuffleQuestions = quizDto.ShuffleQuestions,
            MaxPoints = quizDto.MaxPoints,
            TimeLimitMinutes = quizDto.TimeLimitMinutes,
            Questions = quizDto.Questions.Select(question => _questionFactory.CreateQuestion(question)).ToList(),
            AssignmentType = AssignmentType.Quiz,
            DueDate = quizDto.DueDate,
            SectionId = quizDto.SectionId,
            Attachments = quizDto.Attachments.Select((att) => _attachmentFactory.CreateAttachment(att)).ToList(),
        };
    }

    private Assignment CreateTaskAssignment(CreateAssignmentDto taskDto)
    {
        return new Assignment
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            Attachments = taskDto.Attachments.Select((att) => _attachmentFactory.CreateAttachment(att)).ToList(),
            DueDate = taskDto.DueDate,
            MaxPoints = taskDto.MaxPoints,
            SectionId = taskDto.SectionId,
            AssignmentType = taskDto.AssignmentType
        };
    }
}
