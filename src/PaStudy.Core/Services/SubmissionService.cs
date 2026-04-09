using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Helpers.DTOs.Submission;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Core.Interfaces.Service;

namespace PaStudy.Core.Services;

public class SubmissionService : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IQuizSubmissionService _quizSubmissionService;
    private readonly IStudentRepository _studentRepository;

    public SubmissionService(
        ISubmissionRepository submissionRepository,
        IQuizSubmissionService quizSubmissionService, 
        IStudentRepository studentRepository)
    {
        _submissionRepository = submissionRepository;
        _quizSubmissionService = quizSubmissionService;
        _studentRepository = studentRepository;
    }

    public async Task CreateSubmission(CreateSubmissionDto submissionDto, string userId)
    {
        var student = await _studentRepository.GetByUserIdAsync(userId);
        if (student == null)
            throw new ArgumentException("Student not found for the given user ID.");

        Submission submission;

        switch (submissionDto.AssignmentType)
        {
            case AssignmentType.Task:
                submission = CreateTaskSubmission(submissionDto);
                break;

            case AssignmentType.Quiz:
                if (submissionDto.QuizSubmission == null)
                    throw new ArgumentException("Quiz submission details are required.");

                submission = await _quizSubmissionService.ProcessSubmissionAsync(submissionDto);
                break;

            default:
                throw new NotSupportedException("Unsupported assignment type.");
        }

        InitializeBaseSubmissionFields(submission, submissionDto, student.Id);

        await _submissionRepository.CreateSubmission(submission);
    }

    private void InitializeBaseSubmissionFields(Submission submission, CreateSubmissionDto dto, int studentId)
    {
        submission.StudentId = studentId;
        submission.AssignmentId = dto.AssignmentId;
        submission.Status = SubmissionStatus.Submitted;
        submission.SubmittedAt = DateTime.UtcNow;

        submission.TeacherFeedback = null;
    }

    private TaskSubmission CreateTaskSubmission(CreateSubmissionDto dto)
    {
        return new TaskSubmission
        {
            StudentNotes = dto.TaskSubmission?.StudentNotes ?? string.Empty,
            Attachments = dto.TaskSubmission?.Attachments
                          ?? new List<Entities.Attachments.Attachment>()
        };
    }
}
