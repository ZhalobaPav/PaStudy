using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;

namespace PaStudy.Core.Interfaces.Factories;

public interface IQuestionFactory
{
    Question CreateQuestion(CreateQuestionDto dto);
}
