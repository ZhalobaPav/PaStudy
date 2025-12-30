using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Course;
using PaStudy.Core.Helpers.FilterObjects.CourseFilters;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using System.Collections.Immutable;

namespace PaStudy.Infrastructure.Repositories;
public class CourseRepository: ICourseRepository
{
    private readonly PaStudyDbContext _context;

    public CourseRepository(PaStudyDbContext context)
    {
        _context = context;
    }

    public async Task<ImmutableArray<CourseDto>> GetCourses(CancellationToken cancellationToken, CourseFilter courseFilter)
    {
        IQueryable<Course> courses = _context.Set<Course>().AsNoTracking();
        if(!string.IsNullOrEmpty(courseFilter.SearchTerm))
        {
            var term = courseFilter.SearchTerm.ToLower();
            courses = courses.Where(c => c.Title.Contains(term));
        }
        courses = courses.OrderBy(c => c.Id);
        int pageNumber = courseFilter.PageNumber ?? 1;
        int pageSize = courseFilter.PageSize ?? 10;
        courses = courses.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        var result = await courses.Select(c => new CourseDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            CategoryName = c.Category.Name,
            Teachers = c.TeacherCourses.Select(tc => tc.Teacher.ToTeacherDto()).ToImmutableArray()
        }).ToImmutableArrayAsync(cancellationToken);
        return result;
    }

    public async Task<CourseDto> GetCourseByIdAsync(int id, CancellationToken cancellationToken)
    {
        var course = await _context.Set<Course>()
        .Where(c => c.Id == id)
        .Select(c => new CourseDto()
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            CategoryName = c.Category.Name,
            Teachers = c.TeacherCourses.Select(tc => tc.Teacher.ToTeacherDto()).ToImmutableArray()
        })
        .FirstOrDefaultAsync(cancellationToken);

        return course;
    }
}

