using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Teacher;
using System.Linq.Expressions;

namespace PaStudy.Core.Helpers.Extensions.MapperHelpers;

public static class TeacherMapper
{
    public static BriefTeacherDto ToTeacherDto(this Teacher teacher)
    {
        return new BriefTeacherDto
        {
            Id = teacher.Id,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            MiddleName = teacher.MiddleName
        };
    }

    public static Expression<Func<Teacher, BriefTeacherDto>> ToDto => teacher => new BriefTeacherDto
    {
        Id = teacher.Id,
        FirstName = teacher.FirstName,
        LastName = teacher.LastName,
        MiddleName = teacher.MiddleName
    };
}
