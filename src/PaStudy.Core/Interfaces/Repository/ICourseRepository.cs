using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Course;
using PaStudy.Core.Helpers.FilterObjects.CourseFilters;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Repository
{
    public interface ICourseRepository
    {
        Task<ImmutableArray<CourseDto>> GetCourses(CancellationToken cancellationToken, CourseFilter courseFilter);
        Task<CourseDto> GetCourseByIdAsync(int id, CancellationToken cancellationToken);
    }
}
