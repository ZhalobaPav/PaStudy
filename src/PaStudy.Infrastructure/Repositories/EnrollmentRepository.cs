using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Entities.ConnectionEntities;
using PaStudy.Core.Helpers.DTOs.Enrollment;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using System.Security.Claims;

namespace PaStudy.Infrastructure.Repositories;

public class EnrollmentRepository: IEnrollmentRepository
{
    private readonly PaStudyDbContext _dbContext;

    public EnrollmentRepository(PaStudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateEnrollmentAsync(int courseId, ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException();

        var student = await _dbContext.Set<Student>()
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

        if(student == null) throw new InvalidOperationException("Student not found.");

        var courseExists = await _dbContext.Set<Course>()
            .AnyAsync(c => c.Id == courseId, cancellationToken);

        if(!courseExists) throw new InvalidOperationException("Course not found.");

        var isAlreadyEnrolled = await _dbContext.Set<Enrollment>()
            .AnyAsync(e => e.CourseId == courseId && e.StudentId == student.Id, cancellationToken);

        if (isAlreadyEnrolled) return true;

        var enrollment = new Enrollment
        {
            CourseId = courseId,
            StudentId = student.Id,
            Status = EnrollmentStatus.Active,
            Progress = 0,
            Created = DateTime.UtcNow,
            CreatedBy = userId
        };

        await _dbContext.AddAsync(enrollment, cancellationToken);
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<List<TeacherGradebookDto>> GetCourseGradebookAsync(int courseId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Enrollment>()
            .Where(e => e.CourseId == courseId)
            .OrderBy(e => e.Student.LastName)
            .ThenBy(e => e.Student.FirstName)
            .Select(e => new TeacherGradebookDto
            {
                EnrollmentId = e.Id,
                StudentId = e.StudentId,
                StudentFullName = $"{e.Student.LastName} {e.Student.FirstName} {e.Student.MiddleName}".Trim(),
                GroupName = e.Student.Group != null ? e.Student.Group.GroupNumber : "Без групи",
                FinalGrade = e.FinalGrade,
                Progress = e.Progress,
                Status = e.Status.ToString(),
                EnrolledAt = e.Created.UtcDateTime
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
