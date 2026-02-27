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
        var courses = _context.Set<Course>().AsNoTracking().AsSplitQuery();
        if(!string.IsNullOrEmpty(courseFilter.SearchTerm))
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

