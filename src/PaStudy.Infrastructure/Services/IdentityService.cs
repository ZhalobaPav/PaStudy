using Microsoft.AspNetCore.Identity;
using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs;
using PaStudy.Core.Helpers.DTOs.Identity;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Models;

namespace PaStudy.Infrastructure.Services;

public class IdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly PaStudyDbContext _dbContext;

    public IdentityService(
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IStudentRepository studentRepository,
        ITeacherRepository teacherRepository,
        PaStudyDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
        _dbContext = dbContext;
    }
    public async Task<IdentityResult> RegisterUserAsync(CreateUserDto userDto)
    {
        if (!userDto.Password.Equals(userDto.ConfirmPassword))
        {
            return IdentityResult.Failed(new IdentityError { Description = "Passwords do not match." });
        }
        var user = new ApplicationUser
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber
        };
        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {
            return result;
        }

        var roleName = userDto.Role.ToString();

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        await _userManager.AddToRoleAsync(user, roleName);
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            if (userDto.Equals(UserRole.Student))
            {
                var student = new Student() 
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    MiddleName = userDto.MiddleName,
                    UserId = user.Id,
                    DateOfBirth = userDto.DateOfBirth,
                    GroupId = userDto.GroupId
                };
                await _studentRepository.CreateStudentAsync(student);
            }
            else if (userDto.Role.Equals(UserRole.Teacher))
            {
                var teacherDto = new CreateTeacherDto
                {
                    UserId = user.Id,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    MiddleName = userDto.MiddleName,
                    GroupId = userDto.GroupId
                };
                await _teacherRepository.CreateTeacherAsync(teacherDto);
            }
        }
        catch (Exception ex)
        {
            await _userManager.DeleteAsync(user);
            await transaction.RollbackAsync();

            return IdentityResult.Failed(new IdentityError { Description = $"An error occurred while creating the profile: {ex.Message}" });
        }
        return IdentityResult.Success;
    }
}
