using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs;
using PaStudy.Core.Helpers.DTOs.Teacher;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Repository;

public interface ITeacherRepository
{
    Task<Teacher> CreateTeacherAsync(CreateTeacherDto teacherDto);
    Task<ImmutableArray<TeacherDto>> GetTeachers(CancellationToken cancellationToken);
}
