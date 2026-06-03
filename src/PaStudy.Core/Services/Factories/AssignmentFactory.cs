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
            AssignmentType.Quiz => CreateQuizAssignment(createAssignmentDto),
            AssignmentType.Reading => CreateReadAssignment(createAssignmentDto),
            _ => throw new ArgumentException("Invalid assignment type")
        };
    }

    private QuizAssignment CreateQuizAssignment(CreateAssignmentDto quizDto)
    {
        return new QuizAssignment
        {
            Title = quizDto.Title,
            Description = quizDto.Description,
            ShuffleQuestions = quizDto.QuizInfo.ShuffleQuestions,
            MaxPoints = quizDto.MaxPoints,
            TimeLimitMinutes = quizDto.QuizInfo.TimeLimitMinutes,
            Questions = quizDto.QuizInfo.Questions.Select(question => _questionFactory.CreateQuestion(question)).ToList(),
            AssignmentType = AssignmentType.Quiz,
            DueDate = quizDto.DueDate,
            StartDate = quizDto.StartDate,
            SectionId = quizDto.SectionId,
            Attachments = quizDto.Attachments.Select((att) => _attachmentFactory.CreateAttachment(att)).ToList(),
        };
    }

    private TaskAssignment CreateTaskAssignment(CreateAssignmentDto taskDto)
    {
        return new TaskAssignment
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            Attachments = taskDto.Attachments.Select((att) => _attachmentFactory.CreateAttachment(att)).ToList(),
            DueDate = taskDto.DueDate,
            StartDate = taskDto.StartDate,
            MaxPoints = taskDto.MaxPoints,
            SectionId = taskDto.SectionId,
            AssignmentType = taskDto.AssignmentType
        };
    }

    private ReadAssignment CreateReadAssignment(CreateAssignmentDto readDto)
    {
        return new ReadAssignment
        {
            Title = readDto.Title,
            Description = readDto.Description,
            Attachments = readDto.Attachments.Select((att) => _attachmentFactory.CreateAttachment(att)).ToList(),
            DueDate = readDto.DueDate,
            StartDate = readDto.StartDate,
            MaxPoints = 0,
            SectionId = readDto.SectionId,
            AssignmentType = AssignmentType.Reading
        };
    }
}
