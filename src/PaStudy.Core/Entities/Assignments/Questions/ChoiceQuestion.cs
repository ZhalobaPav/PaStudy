namespace PaStudy.Core.Entities.Assignments.Questions;

public class ChoiceQuestion: Question
{
    public ICollection<AwnserOption> Options { get; set; }
}
