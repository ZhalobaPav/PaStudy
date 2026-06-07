using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Entities.ConnectionEntities;
using PaStudy.Core.Helpers.DTOs.Category;
using PaStudy.Core.Helpers.DTOs.Course;
using PaStudy.Core.Helpers.DTOs.Course.Note;
using PaStudy.Core.Helpers.DTOs.Teacher;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Helpers.Exceptions;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using PaStudy.Core.Helpers.FilterObjects;
using PaStudy.Core.Helpers.FilterObjects.CourseFilters;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PaStudy.Infrastructure.Repositories;
public class CourseRepository : ICourseRepository
{
    private readonly PaStudyDbContext _context;
    private readonly ITeacherRepository _teacherRepository;

    public CourseRepository(PaStudyDbContext context, ITeacherRepository teacherRepository)
    {
        _context = context;
        _teacherRepository = teacherRepository;
    }

    public async Task<ImmutableArray<CourseDto>> GetCourses(CancellationToken cancellationToken, CourseFilter courseFilter, ClaimsPrincipal user)
    {
        var courses = _context.Set<Course>().AsNoTracking().AsSplitQuery();
        if (courseFilter.CourseQuantity == CourseQuantity.Enrolled)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = user.IsInRole("Teacher") ? "Teacher" : "Student";
            if (role == "Student")
            {
                courses = courses.Where(c => c.Enrollments.Any(e => e.Student.UserId == userId));
            }
            else if (role == "Teacher")
            {
                courses = courses.Where(c => c.TeacherCourses.Any(tc => tc.Teacher.UserId == userId));
            }
        }
        if (!string.IsNullOrEmpty(courseFilter.SearchTerm))
        {
            courses = courses.Where(c => c.Title.Contains(courseFilter.SearchTerm));
        }
        int pageNumber = courseFilter.PageNumber ?? 1;
        int pageSize = courseFilter.PageSize ?? 10;
        var result = await courses
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CategoryName = c.Category.Name,
                Teachers = c.TeacherCourses.Select(tc => tc.Teacher).AsQueryable().Select(TeacherMapper.ToDto).ToImmutableArray()
            }).ToImmutableArrayAsync(cancellationToken);
        return result;
    }

    public async Task<CourseDto> GetCourseByIdAsync(int id, CancellationToken cancellationToken, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        var course = await _context.Set<Course>()
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CategoryName = c.Category != null ? c.Category.Name : string.Empty,
                CategoryId = c.CategoryId,
                Teachers = c.TeacherCourses
                    .Select(tc =>
                        new BriefTeacherDto
                        {
                            Id = tc.Teacher.Id,
                            FirstName = tc.Teacher.FirstName,
                            LastName = tc.Teacher.LastName,
                            MiddleName = tc.Teacher.MiddleName
                        }
                    )
                    .ToImmutableArray(),
                IsEnrolled = c.Enrollments.Any(e => e.Student.UserId == userId),
                IsTeaching = c.TeacherCourses.Any(tc => tc.Teacher.UserId == userId)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return course;
    }
    public async Task<ImmutableArray<CategoryBriefDto>> GetCategoryBriefInfo(CancellationToken ct)
    {
        return await _context.Set<Category>()
        .Select(c => new CategoryBriefDto
        {
            Id = c.Id,
            Name = c.Name
        })
        .ToImmutableArrayAsync(ct);
    }
    public async Task<ImmutableArray<NoteDto>> GetNotesAsync(int courseId, CancellationToken cancellationToken, ClaimsPrincipal user, BaseFilterRequest filter)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!user.IsInRole("Student"))
        {
            throw new ForbiddenException("Only students can look at their notes");
        }

        var query = _context.Set<Assignment>()
            .Where(a => a.Section.CourseId == courseId);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(a => a.Title.Contains(filter.SearchTerm));
        }

        int pageNumber = filter.PageNumber ?? 1;
        int pageSize = filter.PageSize ?? 10;

        var dtoQuery = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new
            {
                Assignment = a,
                TaskSub = a.Submissions
                    .Where(s => s.Student.UserId == userId)
                    .Select(s => new { s.Grade, s.TeacherFeedback })
                    .FirstOrDefault(),
                QuizSub = _context.Set<QuizAttempt>()
                    .Where(qa => qa.Quiz.Id == a.Id && qa.UserId == userId && qa.Status == QuizAttemptStatus.Completed)
                    .OrderByDescending(qa => qa.TotalScore)
                    .Select(qa => new { Grade = qa.TotalScore, TeacherFeedback = (string?)null })
                    .FirstOrDefault()
            })
            .Select(x => new NoteDto
            {
                AssignmentInfo = new NoteAssignmentInfo(x.Assignment.MaxPoints, x.Assignment.Title, x.Assignment.Id),
                Grade = x.Assignment.AssignmentType == AssignmentType.Task
                    ? x.TaskSub.Grade
                    : x.QuizSub.Grade,
                TeacherFeadback = x.Assignment.AssignmentType == AssignmentType.Task
                    ? x.TaskSub.TeacherFeedback
                    : null,
                Percentage = 0
            });

        var results = await dtoQuery.ToListAsync(cancellationToken);

        foreach (var note in results)
        {
            if (note.Grade.HasValue && note.AssignmentInfo.MaxPoints > 0)
            {
                note.Percentage = Math.Floor((note.Grade.Value / (decimal)note.AssignmentInfo.MaxPoints) * 100);
            }
        }

        return results.ToImmutableArray();
    }

    public async Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto dto, ClaimsPrincipal user, CancellationToken ct)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (String.IsNullOrEmpty(userId))
        {
            throw new ForbiddenException("You cannot create courses");
        }
        var teacher = await _teacherRepository.GetByUserIdAsync(userId);

        if (teacher == null)
        {
            throw new ForbiddenException("You cannot create courses");
        }
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            TeacherCourses = new List<TeacherCourses>()
        };

        course.TeacherCourses.Add(new TeacherCourses
        {
            TeacherId = teacher.Id,
            Course = course
        });
        _context.Set<Course>().Add(course);
        await _context.SaveChangesAsync(ct);

        return new CourseResponseDto(
            course.Id,
            course.Title,
            course.Description,
            course.CategoryId,
            course.Created
        );

    }

    public async Task<List<int>> GetUserCourseIdsAsync(string userId)
    {
        var studentCourseIds = await _context.Set<Enrollment>()
            .Where(e => e.Student.UserId == userId)
            .Select(e => e.CourseId)
            .ToListAsync();

        if (studentCourseIds.Any())
        {
            return studentCourseIds;
        }

        var teacherCourseIds = await _context.Set<TeacherCourses>()
            .Where(tc => tc.Teacher.UserId == userId)
            .Select(tc => tc.CourseId)
            .ToListAsync();

        return teacherCourseIds;
    }

    public async Task UpdateCourseAsync(UpdateCourseDto updateCourseDto, ClaimsPrincipal user, CancellationToken ct)
    {
        var course = await _context.Set<Course>()
            .Include(c => c.TeacherCourses)
            .ThenInclude(ct => ct.Teacher)
            .FirstOrDefaultAsync(c => c.Id == updateCourseDto.Id, ct);

        if (course == null)
        {
            throw new NotFoundException("Course not found");
        }

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!course.TeacherCourses.Any(tc => tc.Teacher.UserId == userId))
        {
            throw new ForbiddenException("You cannot edit this course");
        }

        course.Title = updateCourseDto.Title;
        course.Description = updateCourseDto.Description;
        course.CategoryId = updateCourseDto.CategoryId;

        await _context.SaveChangesAsync(ct);
    }
}

