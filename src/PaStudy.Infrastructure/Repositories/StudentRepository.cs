using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Group;
using PaStudy.Core.Helpers.DTOs.Student;
using PaStudy.Core.Helpers.FilterObjects.UserFilters;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using PaStudy.Infrastructure.Models;
using System.Collections.Immutable;
using System.Linq;

namespace PaStudy.Infrastructure.Repositories;

public class StudentRepository: IStudentRepository
{
    private readonly PaStudyDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentRepository(PaStudyDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        var students = _context.Set<Student>();
        var groups = _context.Set<Group>();
        bool userAlreadyHasStudent = await students
            .AnyAsync(s => s.UserId == student.UserId);

        if (userAlreadyHasStudent)
            throw new InvalidOperationException("User already has an associated student profile.");

        bool groupExists = await groups
            .AnyAsync(g => g.Id == student.GroupId);

        if (!groupExists)
            throw new ArgumentException($"Group with ID {student.GroupId} does not exist.");

        await students.AddAsync(student);
        await _context.SaveChangesAsync();
        return student;
    }


    public async Task<ImmutableArray<StudentDto>> GetStudents(CancellationToken cancellationToken, UserFilter userFilter)
    {
        var query = _context.Set<Student>().AsQueryable();
        if(userFilter.CourseId.HasValue)
        {
            int courseId = userFilter.CourseId.Value;
            query = query.Where(s => s.Enrollments.Any(sc => sc.CourseId == courseId));
        }
        if (!string.IsNullOrWhiteSpace(userFilter.SearchTerm))
        {
            var term = userFilter.SearchTerm.ToLower();
            query = query.Where(s => s.FirstName.ToLower().Contains(term) ||
                                     s.LastName.ToLower().Contains(term));
        }
        var students = await query
            .Join(_userManager.Users, 
                student => student.UserId, 
                user => user.Id, 
                (student, user) => new { student, user }
            )
            .Select(x => new StudentDto()
            {
                Id = x.student.Id,
                FirstName = x.student.FirstName,
                LastName = x.student.LastName,
                DateOfBirth = x.student.DateOfBirth,
                Email = x.user.Email,
                PhoneNumber = x.user.PhoneNumber,
                Group = new GroupDto
                {
                    Id = x.student.Group.Id,
                    GroupNumber = x.student.Group.GroupNumber,
                    InstitutionNumber = x.student.Group.InstitutionNumber,
                    Faculty = x.student.Group.Faculty,
                    Speciality = x.student.Group.Speciality
                }
            }).ToImmutableArrayAsync(cancellationToken);
        return students;
    }
}
