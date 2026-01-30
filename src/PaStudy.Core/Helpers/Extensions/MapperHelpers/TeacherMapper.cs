using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Teacher;

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
