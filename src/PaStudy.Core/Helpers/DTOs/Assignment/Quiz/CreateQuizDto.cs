namespace PaStudy.Core.Helpers.DTOs.Assignment.Quiz;

public class CreateQuizDto: CreateAssignmentDto
{
    public ICollection<CreateQuestionDto> Questions { get; set; }
    public bool ShuffleQuestions { get; set; }
    public int TimeLimitMinutes { get; set; }
}   
