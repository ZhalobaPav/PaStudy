using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.DTOs.Section;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Helpers.Exceptions;
using PaStudy.Core.Helpers.Exceptions.AssignmentExceptions;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using PaStudy.Core.Interfaces.Factories;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PaStudy.Infrastructure.Repositories;

public class AssignmentRepository: IAssignmentRepository
{
    private readonly PaStudyDbContext _dbContext;
    private readonly IAttachmentFactory _attachmentFactory;
    private readonly IStudentRepository _studentRepository;

    public AssignmentRepository(PaStudyDbContext dbContext, IAttachmentFactory attachmentFactory, IStudentRepository studentRepository)
    {
        _dbContext = dbContext;
        _attachmentFactory = attachmentFactory;
        _studentRepository = studentRepository;
    }
    public async Task<Assignment> CreateAsync(Assignment assignment, CancellationToken ct = default)
    {
        var assignments = _dbContext.Set<Assignment>();
        await assignments.AddAsync(assignment, ct);
        await _dbContext.SaveChangesAsync(ct);
        return assignment;
    } 
    public async Task<Section> CreateSectionAsync(Section section, CancellationToken ct = default)
    {
        var sections = _dbContext.Set<Section>();
        int nextOrder = await sections
            .Where(s => s.CourseId == section.CourseId)
            .Select(s => (int?)s.Order)
            .MaxAsync() ?? 0;
        section.Order = nextOrder + 1;
        await sections.AddAsync(section, ct);
        await _dbContext.SaveChangesAsync(ct);
        return section;
    }
    public async Task<ImmutableArray<SectionDto>> GetSectionsAsync(int courseId, CancellationToken cancellationToken, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException();

        var accessQuery = _dbContext.Set<Course>()
            .AsNoTracking()
            .Where(c => c.Id == courseId);

        if (user.IsInRole("Teacher"))
        {
            accessQuery = accessQuery.Where(c => c.TeacherCourses.Any(tc => tc.Teacher.UserId == userId));
        }
        else
        {
            accessQuery = accessQuery.Where(c => c.Enrollments.Any(e => e.Student.UserId == userId));
        }

        var sectionsData = await accessQuery
            .SelectMany(c => c.Sections)
            .OrderBy(s => s.Order)
            .Select(s => new SectionDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Order = s.Order,
                CourseId = s.CourseId,
                Assignments = s.Assignments.Select(a => new AssignmentDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description ?? string.Empty,
                    DueDate = a.DueDate ?? DateTime.MinValue,
                    StartDate = a.StartDate,
                    MaxPoints = a.MaxPoints,
                    AssignmentType = a.AssignmentType,
                    Attachments = a.Attachments.Select(att => new AttachmentDto
                    {
                        FileName = att.FileName,
                        FileUrl = att.FileUrl,
                        ContentType = att.ContentType,
                        ImageInfo = (att.ContentType == "image/jpeg" || att.ContentType == "image/png" || att.ContentType == "image/gif")
                        ? new ImageAttachmentInfo(
                            ((ImageAttachment)att).Width,
                            ((ImageAttachment)att).Height
                        )
                        : null
                    }).ToImmutableArray(),
                    QuizInfo = a.AssignmentType == AssignmentType.Quiz
                        ? new QuizInfoBrief(
                           ((QuizAssignment)a).ShuffleQuestions,
                           ((QuizAssignment)a).TimeLimitMinutes,
                           ((QuizAssignment)a).Questions.Count
                        )
                : null
                }).ToImmutableArray()
            })
            .ToListAsync(cancellationToken);

        if (sectionsData.Count == 0)
        {
            bool hasAccess = await accessQuery.AnyAsync(cancellationToken);
            if (!hasAccess)
            {
                throw new NotEnrolledException("You do not have access to this course or it doesn't exist.");
            }
        }

        return sectionsData.ToImmutableArray();
    }
    public async Task<ImmutableArray<AssignmentDto>> GetAssignmentsAsync(int courseId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Assignment>().Where(a => a.Section.CourseId == courseId).Select(a => a.ToAssignmentDto())
            .ToImmutableArrayAsync(cancellationToken);
    }
    public async Task AddAttachmentsToAssignment(ICollection<CreateAttachmentDto> createAttachmentDtoList, int assignmentId)
    {
        var assignment = await _dbContext.Set<Assignment>()
        .Include(a => a.Attachments)
        .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null) throw new NotFoundException("Assignment not found");
        var attachments = createAttachmentDtoList.Select(a => _attachmentFactory.CreateAttachment(a));
        foreach (var attachment in attachments)
        {
            assignment.Attachments.Add(attachment);
        }
        await _dbContext.SaveChangesAsync();
    }
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "attachments");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/attachments/{fileName}";
    }

    public async Task<StudentQuizDto?> GetQuizForPassingAsync(int quizId, CancellationToken cancellation)
    {
        var quiz = await _dbContext.Set<QuizAssignment>()
            .AsNoTracking()
            .Include(q => q.Questions)
                .ThenInclude(q => (q as ChoiceQuestion).Options)
            .Include(q => q.Attachments)
            .FirstOrDefaultAsync(q => q.Id == quizId, cancellation);

        if (quiz == null) return null;

        var questions = quiz.Questions.Select(q =>
        {
            StudentChoiceInfo? choiceInfo = null;
            StudentMatchingInfo? matchingInfo = null;

            if (q is ChoiceQuestion cq)
            {
                var options = cq.Options
                    .Select(o => new StudentAnswerOption(o.Id, o.Text))
                    .OrderBy(_ => Guid.NewGuid())
                    .ToList();
                choiceInfo = new StudentChoiceInfo(options);
            }
            else if (q is MatchingQuestion mq)
            {
                var pairs = mq.Pairs.ToList();

                var leftSide = pairs
                    .Select(p => p.LeftSide)
                    .OrderBy(_ => Guid.NewGuid())
                    .ToList();

                var rightSide = pairs
                    .Select(p => p.RightSide)
                    .OrderBy(_ => Guid.NewGuid())
                    .ToList();

                matchingInfo = new StudentMatchingInfo(leftSide, rightSide);
            }

            return new StudentQuestionDto(
                q.Id,
                q.Text,
                q.Points,
                q.Type,
                choiceInfo,
                matchingInfo,
                q.Attachments.Select(a => new AttachmentDto
                {
                    FileName = a.FileName,
                    FileUrl = a.FileUrl,
                    ContentType = a.ContentType
                }).ToList()
            );
        }).ToImmutableArray();

        if (quiz.ShuffleQuestions)
        {
            questions = questions.OrderBy(_ => Guid.NewGuid()).ToImmutableArray();
        }

        return new StudentQuizDto(
            quiz.Id,
            quiz.Title,
            quiz.Description,
            quiz.TimeLimitMinutes,
            quiz.DueDate,
            quiz.MaxPoints,
            questions,
            quiz.Attachments.Select(a => new AttachmentDto
            {
                FileName = a.FileName,
                FileUrl = a.FileUrl
            }).ToImmutableArray()
        );
    }

    public async Task<AssignmentDto> GetAssignmentByIdAsync(int assignmentId, CancellationToken cancellationToken, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var dto = await _dbContext.Set<Assignment>()
            .AsNoTracking()
            .Where(a => a.Id == assignmentId)
            .Select(a => new AssignmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description ?? string.Empty,
                DueDate = a.DueDate ?? DateTime.MinValue,
                StartDate = a.StartDate,
                MaxPoints = a.MaxPoints,
                AssignmentType = a.AssignmentType,

                Attachments = a.Attachments.Select(att => new AttachmentDto
                {
                    FileName = att.FileName,
                    FileUrl = att.FileUrl,
                    ContentType = att.ContentType,
                    ImageInfo = (att.ContentType == "image/jpeg" || att.ContentType == "image/png" || att.ContentType == "image/gif")
                            ? new ImageAttachmentInfo(
                                ((ImageAttachment)att).Width,
                                ((ImageAttachment)att).Height) : null
                }).ToImmutableArray(),

                QuizInfo = a.AssignmentType == AssignmentType.Quiz
                    ? new QuizInfoBrief(
                       ((QuizAssignment)a).ShuffleQuestions,
                       ((QuizAssignment)a).TimeLimitMinutes,
                       ((QuizAssignment)a).Questions.Count
                    )
                    : null,
                SubmissionInfo = a.Submissions
                    .Where(s => s.Student.UserId == userId)
                    .Select(s => new SubmissionInfo(
                        s.Status != SubmissionStatus.Draft && s.Status != SubmissionStatus.Rejected,
                        a.AssignmentType == AssignmentType.Task ? new TaskSubmissionDto((
                        (TaskSubmission)s).StudentNotes, 
                        ((TaskSubmission)s).Attachments.Select(att => new AttachmentDto
                            {
                                FileName = att.FileName,
                                FileUrl = att.FileUrl,
                                ContentType = att.ContentType,
                                ImageInfo = (att.ContentType == "image/jpeg" || att.ContentType == "image/png" || att.ContentType == "image/gif")
                                        ? new ImageAttachmentInfo(
                                            ((ImageAttachment)att).Width,
                                            ((ImageAttachment)att).Height) : null
                            }).ToImmutableArray()) : null,
                        s.SubmittedAt,
                        s.Grade,
                        s.TeacherFeedback
                    ))
                    .FirstOrDefault()

            })
            .FirstOrDefaultAsync(cancellationToken);

        if (dto == null)
            throw new NotFoundException("Assignment not found");

        return dto;
    }
}
