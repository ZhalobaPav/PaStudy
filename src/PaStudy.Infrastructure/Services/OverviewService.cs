using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Identity;
using PaStudy.Core.Helpers.DTOs.Profile;
using PaStudy.Core.Helpers.FilterObjects.CourseFilters;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Infrastructure.Helpers;
using System.Security.Claims;

namespace PaStudy.Infrastructure.Services;

public class OverviewService: IOverviewService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherRepository _teacherRepository;

    public OverviewService(ICourseRepository courseRepository, IStudentRepository studentRepository, ITeacherRepository teacherRepository)
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<BaseProfileDto> GetProfileInfo(CourseFilter courseFilter, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var roleClaim = user.FindFirst(ClaimTypes.Role).Value;
        var role = IdentityHelper.GetUserRoleNumber(roleClaim);
        if (role == null) 
        {
            throw new UnauthorizedAccessException("You cannot get this info before authorization");
        }
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) 
        {
            throw new UnauthorizedAccessException("You cannot get this info before authentication");
        }
        var fullName = "";
        if(role == UserRole.Student)
        {
            var student = await _studentRepository.GetStudentByUserId(userId);
            fullName = student.LastName + " " + student.FirstName;
        } 
        else
        {
            var teacher = await _teacherRepository.GetByUserIdAsync(userId);
            fullName = teacher.LastName + " " + teacher.FirstName;
        }
        
        var email = user.FindFirst(ClaimTypes.Email)?.Value;
        courseFilter.CourseQuantity = CourseQuantity.Enrolled;
        var courses = await _courseRepository.GetCourses(cancellationToken, courseFilter, user);
        return new BaseProfileDto
        {
            FullName = fullName,
            UserRole = role.Value,
            Email = email,
            Courses = courses,
        };
    }
}
