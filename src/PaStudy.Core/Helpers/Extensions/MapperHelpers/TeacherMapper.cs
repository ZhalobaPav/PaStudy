using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Identity;
using PaStudy.Core.Helpers.DTOs.Student;
using PaStudy.Core.Helpers.DTOs.Teacher;
using PaStudy.Core.Helpers.DTOs.Users;

namespace PaStudy.Core.Helpers.Extensions.MapperHelpers;

public static class TeacherMapper
{
    public static BriefTeacherDto ToTeacherDto(this Teacher student)
    {
        return new BriefTeacherDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            MiddleName = student.MiddleName
        };
    }
}
