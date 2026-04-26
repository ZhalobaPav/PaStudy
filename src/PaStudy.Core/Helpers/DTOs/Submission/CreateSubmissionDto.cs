using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.Enums;
namespace PaStudy.Core.Helpers.DTOs.Submission;
using SumbissionAttachment = Entities.Attachments.Attachment;
public record CreateSubmissionDto
{
    public int AssignmentId { get; set; }
    public AssignmentType AssignmentType { get; set; }
    public CreateTaskSubmission? TaskSubmission { get; set; }
    public CreateQuizSubmission? QuizSubmission { get; set; }

}
public record CreateTaskSubmission(string StudentNotes, ICollection<CreateAttachmentDto> Attachments);
public record CreateQuizSubmission(TimeSpan? TimeTaken, int AttemptNumber, ICollection<CreateQuestionAnswerDto> Answers);

public abstract record CreateQuestionAnswerDto(int QuestionId);

public record CreateChoiceAnswerDto(int QuestionId, int SelectedOptionId)
    : CreateQuestionAnswerDto(QuestionId);

public record CreateMatchingAnswerDto(int QuestionId, List<CreateMatchingPairAnswerDto> SelectedPairs)
    : CreateQuestionAnswerDto(QuestionId);

public record CreateMatchingPairAnswerDto(int MatchingPairId, string SelectedValue);