using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Student;
using PaStudy.Core.Helpers.FilterObjects.UserFilters;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PaStudy.Core.Interfaces.Repository;

public interface IStudentRepository
{
    Task<ImmutableArray<StudentDto>> GetStudents(CancellationToken cancellationToken, UserFilter userFilter);
    Task<Student> CreateStudentAsync(Student student);
    Task<StudentDto> GetStudentByUserId(string userId);
    Task<Student?> GetByUserIdAsync(string userId, CancellationToken ct = default);
}
