using PaStudy.Core.Helpers.DTOs.Identity;
using System.Security.Claims;
using PaStudy.Core.Helpers.StaticData;
namespace PaStudy.Infrastructure.Helpers;

public static class IdentityHelper
{
    public static UserRole? GetUserRoleNumber(string role)
    {
        return role switch
        {
            BaseConsts.StudentRole => UserRole.Student,
            BaseConsts.TeacherRole => UserRole.Teacher,
            _ => null
        };

    }
}
