using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Group;
using PaStudy.Core.Helpers.DTOs.Identity;
using PaStudy.Core.Helpers.DTOs.Student;
using PaStudy.Core.Helpers.DTOs.Teacher;
using PaStudy.Core.Helpers.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaStudy.Core.Helpers.Extensions.MapperHelpers;

public static class UserProfileMapper
{
    public static UserProfileResponseDto ToUserProfile(this StudentDto student)
    {
        return new UserProfileResponseDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Role = UserRole.Student,
            PhoneNumber = student.PhoneNumber,
            Group = student.Group,
            DateOfBirth = student.DateOfBirth
        };
    }

    public static UserProfileResponseDto ToUserProfile(this TeacherDto teacher)
    {
        return new UserProfileResponseDto
        {
            Id = teacher.Id,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Email = teacher.Email,
            Role = UserRole.Teacher,
            PhoneNumber = teacher.PhoneNumber,
            Group = teacher.Group
        };
    }
}
