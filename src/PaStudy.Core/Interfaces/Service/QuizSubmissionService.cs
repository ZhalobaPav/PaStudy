using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Helpers.DTOs.Submission;
using PaStudy.Core.Interfaces.Factories;
using PaStudy.Core.Interfaces.Repository;

namespace PaStudy.Core.Interfaces.Service;

public class QuizSubmissionService: IQuizSubmissionService
{
    private readonly IQuestionAnswerFactory _answerFactory;
    private readonly IScoringService _scoringService;
    private readonly IQuestionRepository _questionRepository;

    public QuizSubmissionService(
        IQuestionAnswerFactory answerFactory,
        IScoringService scoringService,
        IQuestionRepository questionRepository)
    {
        _answerFactory = answerFactory;
        _scoringService = scoringService;
        _questionRepository = questionRepository;
    }

    public async Task<QuizSubmission> ProcessSubmissionAsync(CreateSubmissionDto dto)
    {
        var quizDto = dto.QuizSubmission;
        if (quizDto == null) throw new ArgumentException("Quiz data is missing");

        var questionIds = quizDto.Answers.Select(a => a.QuestionId).ToList();
        var questions = (await _questionRepository.GetByIdsAsync(questionIds))
                        .ToDictionary(q => q.Id);

        var submission = new QuizSubmission
        {
            AttemptNumber = quizDto.AttemptNumber,
            TimeTaken = quizDto.TimeTaken,
            Answers = new List<QuestionAnswer>()
        };

        decimal totalGrade = 0;

        foreach (var answerDto in quizDto.Answers)
        {
            if (!questions.TryGetValue(answerDto.QuestionId, out var question)) continue;

            var answer = _answerFactory.CreateAnswer(question, answerDto);

            answer.PointsAwarded = _scoringService.CalculateScore(question, answer);

            totalGrade += answer.PointsAwarded;
            submission.Answers.Add(answer);
        }

        submission.Grade = totalGrade;

        return submission;
    }
}
