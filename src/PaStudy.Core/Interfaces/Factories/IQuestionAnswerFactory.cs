using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Entities.Assignments.Submission;

namespace PaStudy.Core.Interfaces.Factories;

public interface IQuestionAnswerFactory
{
    QuestionAnswer CreateAnswer(Question question, object rawData);
}
