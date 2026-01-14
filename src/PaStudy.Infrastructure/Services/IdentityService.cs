using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs;
using PaStudy.Core.Helpers.DTOs.Identity;
using PaStudy.Core.Helpers.DTOs.Teacher;
using PaStudy.Core.Helpers.DTOs.Users;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PaStudy.Infrastructure.Services;

public class IdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly PaStudyDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IStudentRepository studentRepository,
        ITeacherRepository teacherRepository,
        PaStudyDbContext dbContext, 
        IConfiguration configuration,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
        _dbContext = dbContext;
        _configuration = configuration;
        _signInManager = signInManager;
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
            if (userDto.Role.Equals(UserRole.Student))
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
        await transaction.CommitAsync();
        return IdentityResult.Success;
    }

    public async Task<AuthResultDto> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
        if (user == null)
        {
            return new AuthResultDto { Succeeded = false, Errors = new[] { "Користувача не знайдено" } };
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
        if (!result.Succeeded)
        {
            return new AuthResultDto { Succeeded = false, Errors = new[] { "Невірний пароль" } };
        }
        var token = GenerateToken(user);
        return new AuthResultDto
        {
            Succeeded = true,
            Token = token
        };
    }

    public string GenerateToken(ApplicationUser user)
    {
        var jwtOptions = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userName", user.UserName!)
        };

        var token = new JwtSecurityToken(
            issuer: jwtOptions["Issuer"],
            audience: jwtOptions["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtOptions["DurationInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
