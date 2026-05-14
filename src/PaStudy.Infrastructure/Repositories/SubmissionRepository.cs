using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Entities.ConnectionEntities;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.DTOs.Submission;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Helpers.Exceptions;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using PaStudy.Core.Helpers.FilterObjects.Submissions;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using PaStudy.Infrastructure.Migrations;
using System.Collections.Immutable;
using System.Security.Claims;
using MassTransit;
using PaStudy.Contracts.Commands;
using PaStudy.Core.Helpers.DTOs.Notification;
using PaStudy.Core.Entities.Notification;

namespace PaStudy.Infrastructure.Repositories;

public class SubmissionRepository: ISubmissionRepository
{
    private readonly PaStudyDbContext _dbContext;
    private readonly ITeacherRepository _teacherRepository;
    private readonly INotificationRepository _notificationRepository;

    public SubmissionRepository(PaStudyDbContext dbContext, ITeacherRepository teacherRepository, INotificationRepository notificationRepository)
    {
        _dbContext = dbContext;
        _teacherRepository = teacherRepository;
        _notificationRepository = notificationRepository;
    }
    public async Task<ImmutableArray<SubmissionListItemDto>> GetSubmissionsByAssignmentIdAsync(SubmissionFilter filter, CancellationToken cancellationToken)
    {
        int pageNumber = filter.PageNumber ?? 1;
        int pageSize = filter.PageSize ?? 10;
        return await _dbContext.Set<Submission>()
            .AsNoTracking()
            .Where(s => s.AssignmentId == filter.AssignmentId)
            .Select(s => new SubmissionListItemDto
            {
                Id = s.Id,
                StudentId = s.StudentId,
                StudentFullName = (s.Student.LastName + " " + s.Student.FirstName + " " + s.Student.MiddleName).Trim(),
                SubmittedAt = s.SubmittedAt,
                Grade = s.Grade,
                Status = s.Status
            })
            .OrderByDescending(s => s.SubmittedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToImmutableArrayAsync(cancellationToken);
    }

    public async Task<TaskSubmissionDto> GradeSubmissionAsync(GradeSubmissionDto dto, ClaimsPrincipal user)
    {
        var submission = await _dbContext.Set<Submission>()
        .Include(s => s.Assignment)
            .ThenInclude(a => a.Section)
                .ThenInclude(sec => sec.Course)
        .Include(s => s.Student)
        .FirstOrDefaultAsync(s => s.Id == dto.SubmissionId);
        if (submission == null)
        {
            throw new ArgumentException($"Submission with id {dto.SubmissionId} not found");
        }
        var courseId = submission.Assignment.Section.CourseId;
        var courseTitle = submission.Assignment.Section.Course.Title;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)){
            throw new UnauthorizedAccessException("User must be authenticated to grade submissions");
        }
        var canManage = await _teacherRepository.CanUserManageCourse(user, courseId);

        if (!canManage)
        {
            throw new ForbiddenException("You do not have permission to grade this submission");
        }

        if (dto.Grade < 0 || dto.Grade > submission.Assignment.MaxPoints)
            throw new BadRequestException($"Grade must be between 0 and {submission.Assignment.MaxPoints}");

        submission.Grade = dto.Grade;
        submission.TeacherFeedback = dto.TeacherFeedback;
        submission.GradedAt = DateTime.UtcNow;
        submission.Status = SubmissionStatus.Graded;
        await _dbContext.SaveChangesAsync();
        await UpdateCourseProgress(submission.StudentId, courseId);
        await _notificationRepository.AddNotificationAsync(new CreateNotificationDto
        {
            Title = "Роботу оцінено! 📝",
            Message = $"Вашу роботу з завдання \"{submission.Assignment.Title}\" було оцінено.",
            Type = NotificationType.GradeReceived,
            RecipientUserId = submission.Student.UserId,
            CourseId = courseId,
            ClickActionUrl = $"/courses/course-details/{courseId}"
        });
        return SubmissionMapping.MapToTaskDto(submission, user);
    }

    public async Task UpdateCourseProgress(int studentId, int courseId)
    {
        if (courseId == 0) return;

        var enrollment = await _dbContext.Set<Enrollment>()
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        if (enrollment == null) return;

        var student = await _dbContext.Set<Student>().FindAsync(studentId);
        if (student == null) return;
        string userId = student.UserId;

        var totalAssignmentsCount = await _dbContext.Set<Assignment>()
            .Where(a => a.Section.CourseId == courseId)
            .CountAsync();

        var taskGrades = await _dbContext.Set<Submission>()
            .Where(s => s.StudentId == studentId &&
                        s.Assignment.Section.CourseId == courseId &&
                        s.Status == SubmissionStatus.Graded)
            .Select(s => s.Grade)
            .ToListAsync();

        int completedTasksCount = taskGrades.Count;
        decimal tasksTotalPoints = taskGrades.Sum(g => g ?? 0);

        var quizAttempts = await _dbContext.Set<QuizAttempt>()
            .Where(a => a.UserId == userId &&
                        a.Quiz.Section.CourseId == courseId &&
                        a.Status == QuizAttemptStatus.Completed)
            .GroupBy(a => a.QuizId)
            .Select(group => group.Max(a => a.TotalScore ?? 0))
            .ToListAsync();

        int completedQuizzesCount = quizAttempts.Count;
        decimal quizzesTotalPoints = quizAttempts.Sum();

        if (totalAssignmentsCount > 0)
        {
            int totalCompletedActivities = completedTasksCount + completedQuizzesCount;

            enrollment.Progress = (double)totalCompletedActivities / totalAssignmentsCount * 100;
            enrollment.FinalGrade = tasksTotalPoints + quizzesTotalPoints;

            if (enrollment.Progress >= 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
            }
        }

        _dbContext.Set<Enrollment>().Update(enrollment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TaskSubmissionDto> GetSubmissionByIdAsync(int id, ClaimsPrincipal user)
    {
        if (!user.IsInRole("Teacher"))
        {
            throw new UnauthorizedAccessException("Only teachers can access submission details");
        }

        var query = from s in _dbContext.Set<Submission>()
                    join u in _dbContext.Users on s.Student.UserId equals u.Id
                    where s.Id == id
                    select new TaskSubmissionDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        StudentNotes = ((TaskSubmission)s).StudentNotes,
                        Attachments = ((TaskSubmission)s).Attachments.Select(a => new AttachmentDto
                        {
                            ContentType = a.ContentType,
                            FileName = a.FileName,
                            FileUrl = a.FileUrl,
                        }).ToList(),
                        Grade = s.Grade,
                        TeacherFeedback = s.TeacherFeedback,
                        Status = s.Status,
                        StudentInfo = new StudentInfo(
                            (s.Student.LastName + " " + s.Student.FirstName + " " + s.Student.MiddleName).Trim(),
                            u.Email
                        ), 
                        AssignmentInfo = new AssignmentInfo(
                            s.Assignment.Title,
                            s.Assignment.Description,
                            s.Assignment.DueDate,
                            s.Assignment.MaxPoints
                        )
                    };

        var result = await query.AsNoTracking().FirstOrDefaultAsync();

        if (result == null)
        {
            throw new ArgumentException($"Submission with id {id} not found");
        }

        return result;
    }
    public async Task CreateSubmission(Submission submission)
    {
        if(submission == null)
        {
            throw new ArgumentNullException(nameof(submission));
        }

        await _dbContext.Set<Submission>().AddAsync(submission);
        await _dbContext.SaveChangesAsync();
    }
}
