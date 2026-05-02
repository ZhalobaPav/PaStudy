using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Entities.Base;

namespace PaStudy.Core.Entities.Assignments;

public class QuizAttempt: BaseAuditableEntity
{
    public int QuizId { get; set; }
    public QuizAssignment Quiz { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedAt { get; set; }

    public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.InProgress;

    public decimal? TotalScore { get; set; }

    public ICollection<QuizAttemptAnswer> Answers { get; set; } = new List<QuizAttemptAnswer>();

    public bool IsExpired =>
        Status == QuizAttemptStatus.InProgress &&
        SubmittedAt == null &&
        DateTime.UtcNow > StartedAt.AddMinutes(Quiz.TimeLimitMinutes);
}
public enum QuizAttemptStatus
{
    InProgress,
    Submitted,
    Completed, 
    Expired
}

public class QuizAttemptAnswer: BaseEntity
{
    public int QuizAttemptId { get; set; }
    public QuizAttempt Attempt { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public int? SelectedOptionId { get; set; }
    public List<int>? SelectedOptionIds { get; set; }
    public Dictionary<string, string>? MatchingAnswers { get; set; }
    public string? TextResponse { get; set; }
    public bool IsCorrect { get; set; } 
    public decimal PointsEarned { get; set; }
}