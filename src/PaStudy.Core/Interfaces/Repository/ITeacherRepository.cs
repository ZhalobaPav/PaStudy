using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Teacher;
using PaStudy.Core.Helpers.FilterObjects.UserFilters;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PaStudy.Core.Interfaces.Repository;

public interface ITeacherRepository
{
    Task<Teacher> CreateTeacherAsync(CreateTeacherDto teacherDto);
    Task<ImmutableArray<TeacherDto>> GetTeachers(CancellationToken cancellationToken, UserFilter userFilter);
    Task<Teacher?> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<Teacher?> GetByIdAsync(int teacherId, CancellationToken ct = default);
    Task<bool> CanTeacherManageCourse(int teacherId, int courseId);
    Task<bool> CanUserManageCourse(ClaimsPrincipal user, int courseId);
    bool IsTeacher(ClaimsPrincipal user);
}
