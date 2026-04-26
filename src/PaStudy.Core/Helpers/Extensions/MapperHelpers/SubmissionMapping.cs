using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.DTOs.Submission;
using System.Security.Claims;

namespace PaStudy.Core.Helpers.Extensions.MapperHelpers;

public static class SubmissionMapping
{
    public static TaskSubmissionDto MapToTaskDto(Submission s, ClaimsPrincipal user)
    {
        var taskSub = s as TaskSubmission;
        var emailClaim = user.FindFirst(ClaimTypes.Email);
        string email = emailClaim?.Value;
        return new TaskSubmissionDto
        {
            Id = s.Id,
            SubmittedAt = s.SubmittedAt,
            Grade = s.Grade,
            TeacherFeedback = s.TeacherFeedback,
            Status = s.Status,
            StudentNotes = taskSub?.StudentNotes,


            StudentInfo = new StudentInfo(
                (s.Student.LastName + " " + s.Student.FirstName + " " + s.Student.MiddleName).Trim(),
                email
            ),

            AssignmentInfo = new AssignmentInfo(
                s.Assignment.Title,
                s.Assignment.Description ?? string.Empty,
                s.Assignment.DueDate,
                s.Assignment.MaxPoints
            ),

            Attachments = taskSub?.Attachments.Select(a => new AttachmentDto
            {
                FileName = a.FileName,
                FileUrl = a.FileUrl,
                ContentType = a.ContentType
            }).ToList() ?? new List<AttachmentDto>()
        };
    }
}
