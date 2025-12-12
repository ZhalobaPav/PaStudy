using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Student;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Repository;

public interface IStudentRepository
{
    Task<ImmutableArray<StudentDto>> GetStudents(CancellationToken cancellationToken);
    Task<Student> CreateStudentAsync(Student student);
}
