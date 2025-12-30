using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs;
using PaStudy.Core.Helpers.DTOs.Group;
using PaStudy.Core.Helpers.DTOs.Teacher;
using PaStudy.Core.Helpers.FilterObjects.UserFilters;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using PaStudy.Infrastructure.Models;
using System.Collections.Immutable;

namespace PaStudy.Infrastructure.Repositories;

public class TeacherRepository: ITeacherRepository
{
    private readonly PaStudyDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public TeacherRepository(PaStudyDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    public async Task<ImmutableArray<TeacherDto>> GetTeachers(CancellationToken cancellationToken, UserFilter userFilter)
    {
        var query = _dbContext.Set<Teacher>().AsQueryable();
        if (!string.IsNullOrWhiteSpace(userFilter.SearchTerm))
        {
            var term = userFilter.SearchTerm.ToLower();
            query = query.Where(t => t.FirstName.ToLower().Contains(term) ||
                                     t.LastName.ToLower().Contains(term) ||
                                     t.MiddleName.ToLower().Contains(term));
        }
        if(userFilter.CourseId.HasValue)
        {
            query = query.Where(t => t.TeacherCourses.Any(c => c.CourseId == userFilter.CourseId.Value));
        }
        var teachers = await query
            .Include(t => t.GroupOfCurator)
            .Join(
                _userManager.Users,
                teacher => teacher.UserId,
                user => user.Id,
                (teacher, user) => new { teacher, user })
            .Select(x => new TeacherDto()
            {
                Id = x.teacher.Id,
                FirstName = x.teacher.FirstName,
                LastName = x.teacher.LastName,
                MiddleName = x.teacher.MiddleName,
                Group = new GroupDto()
                {
                    Id = x.teacher.GroupOfCurator.Id,
                    GroupNumber = x.teacher.GroupOfCurator.GroupNumber,
                    InstitutionNumber = x.teacher.GroupOfCurator.InstitutionNumber,
                    Faculty = x.teacher.GroupOfCurator.Faculty,
                    Speciality = x.teacher.GroupOfCurator.Speciality
                },
                Email = x.user.Email,
                PhoneNumber = x.user.PhoneNumber
            }).ToImmutableArrayAsync(cancellationToken);
        return teachers;
    }
    public async Task<Teacher> CreateTeacherAsync(CreateTeacherDto teacherDto)
    {
        var teachers = _dbContext.Set<Teacher>();
        var groups = _dbContext.Set<Group>();
        bool userAlreadyHasTeacher = await teachers
            .AnyAsync(s => s.UserId == teacherDto.UserId);

        if (userAlreadyHasTeacher)
            throw new InvalidOperationException("User already has an associated student profile.");

        bool groupExists = await groups
            .AnyAsync(g => g.Id == teacherDto.GroupId);

        if (!groupExists)
            throw new ArgumentException($"Group with ID {teacherDto.GroupId} does not exist.");

        var teacher = new Teacher
        {
            UserId = teacherDto.UserId,
            LastName = teacherDto.LastName,
            FirstName = teacherDto.FirstName,
            MiddleName = teacherDto.MiddleName,
            GroupId = teacherDto.GroupId
        };

        await teachers.AddAsync(teacher);
        await _dbContext.SaveChangesAsync();
        return teacher;
    }
}
