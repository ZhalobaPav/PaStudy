using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Helpers.DTOs.Course;
using PaStudy.Core.Helpers.DTOs.Course.Note;
using PaStudy.Core.Helpers.DTOs.Teacher;
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

    public CourseRepository(PaStudyDbContext context)
    {
        _context = context;
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

    public async Task<ImmutableArray<NoteDto>> GetNotesAsync(int courseId, CancellationToken cancellationToken, ClaimsPrincipal user, BaseFilterRequest filter)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var isStudent = user.IsInRole("Student");
        if(!isStudent)
        {
            throw new ForbiddenException("Only students can look it's notes");
        }
        var query = _context.Set<Submission>()
            .Where(s => s.Assignment.Section.CourseId == courseId && s.Student.UserId == userId);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(s => s.Assignment.Title.Contains(filter.SearchTerm));
        }

        int pageNumber = filter.PageNumber ?? 1;
        int pageSize = filter.PageSize ?? 10;
        var dtoQuery = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new NoteDto
            {
                TeacherFeadback = s.TeacherFeedback,
                Grade = s.Grade,
                Percentage = s.Grade.HasValue && s.Assignment.MaxPoints > 0
                    ? Math.Floor((s.Grade.Value / (decimal)s.Assignment.MaxPoints) * 100)
                    : 0,
                AssignmentInfo = new NoteAssignmentInfo(s.Assignment.MaxPoints, s.Assignment.Title)
            });

        return await dtoQuery.ToImmutableArrayAsync(cancellationToken);
    }
}

