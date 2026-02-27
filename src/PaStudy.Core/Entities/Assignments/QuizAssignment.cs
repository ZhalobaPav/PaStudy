using PaStudy.Core.Entities.Assignments.Questions;

namespace PaStudy.Core.Entities.Assignments;

public class QuizAssignment: Assignment
{
    public string Title { get; set; } = string.Empty;
    public bool ShuffleQuestions { get; set; } = false;

    public int TimeLimitMinutes { get; set; }

    public ICollection<Question> Questions { get; set; } = new HashSet<Question>();

}
