namespace PaStudy.Core.Entities.Assignments.Submission;

public class QuizSubmission : Submission
{
    public ICollection<QuestionAnswer> Answers { get; set; } = new List<QuestionAnswer>();
    public TimeSpan? TimeTaken { get; set; }
    public int AttemptNumber { get; set; } = 1;
}
