using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Category;
using PaStudy.Core.Helpers.DTOs.Course;
using PaStudy.Core.Helpers.DTOs.Course.Note;
using PaStudy.Core.Helpers.FilterObjects;
using PaStudy.Core.Helpers.FilterObjects.CourseFilters;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PaStudy.Core.Interfaces.Repository
{
    public interface ICourseRepository
    {
        Task<ImmutableArray<CourseDto>> GetCourses(CancellationToken cancellationToken, CourseFilter courseFilter, ClaimsPrincipal user);
        Task<CourseDto> GetCourseByIdAsync(int id, CancellationToken cancellationToken, ClaimsPrincipal user);
        Task<ImmutableArray<NoteDto>> GetNotesAsync(int courseId, CancellationToken cancellationToken, ClaimsPrincipal user, BaseFilterRequest filter);
        Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto dto, ClaimsPrincipal user, CancellationToken ct);
        Task<ImmutableArray<CategoryBriefDto>> GetCategoryBriefInfo(CancellationToken ct);
        Task<List<int>> GetUserCourseIdsAsync(string userId);
        Task UpdateCourseAsync(UpdateCourseDto updateCourseDto, ClaimsPrincipal user, CancellationToken ct);
    }
}
