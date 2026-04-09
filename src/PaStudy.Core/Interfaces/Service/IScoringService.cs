using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Entities.Assignments.Submission;

namespace PaStudy.Core.Interfaces.Service;

public interface IScoringService
{
    decimal CalculateScore(Question question, QuestionAnswer answer);
}
