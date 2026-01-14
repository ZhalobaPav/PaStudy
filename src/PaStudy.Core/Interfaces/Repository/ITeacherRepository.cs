using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Teacher;
using PaStudy.Core.Helpers.FilterObjects.UserFilters;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Repository;

public interface ITeacherRepository
{
    Task<Teacher> CreateTeacherAsync(CreateTeacherDto teacherDto);
    Task<ImmutableArray<TeacherDto>> GetTeachers(CancellationToken cancellationToken, UserFilter userFilter);
    Task<Teacher?> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<bool> CanTeacherManageCourse(int teacherId, int courseId);
}
