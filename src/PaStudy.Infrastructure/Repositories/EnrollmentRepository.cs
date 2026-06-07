using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Entities.ConnectionEntities;
using PaStudy.Core.Helpers.DTOs.Enrollment;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Models;
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

    public async Task<BulkEnrollmentResult> BulkEnrollStudentsByEmailsAsync(
    int courseId,
    List<string> emails,
    ClaimsPrincipal user,
    CancellationToken cancellationToken)
    {
        var currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentUserId)) throw new UnauthorizedAccessException();

        // 1. Перевіряємо чи існує курс
        var courseExists = await _dbContext.Set<Course>()
            .AnyAsync(c => c.Id == courseId, cancellationToken);

        if (!courseExists) throw new KeyNotFoundException($"Курс з ID {courseId} не знайдено.");

        // 2. Робимо явний Join між студентами та таблицею користувачів Identity
        // Фільтруємо на етапі запиту лише тих, чий Email є у нашому списку
        var students = await _dbContext.Set<Student>()
            .Join(_dbContext.Set<ApplicationUser>(), // або твій кастомний клас юзера, або IdentityUser
                student => student.UserId,
                identityUser => identityUser.Id,
                (student, identityUser) => new { student, identityUser })
            .Where(joined => emails.Contains(joined.identityUser.Email))
            .Select(joined => joined.student) // Нам потрібні сутності студентів для створення зв'язку
            .ToListAsync(cancellationToken);

        if (!students.Any())
        {
            return new BulkEnrollmentResult { Message = "Не знайдено жодного студента з вказаними імейлами." };
        }

        var studentIds = students.Select(s => s.Id).ToList();
        var alreadyEnrolledIds = await _dbContext.Set<Enrollment>()
            .Where(e => e.CourseId == courseId && studentIds.Contains(e.StudentId))
            .Select(e => e.StudentId)
            .ToListAsync(cancellationToken);

        var newEnrollments = new List<Enrollment>();
        int skippedCount = 0;

        foreach (var student in students)
        {
            if (alreadyEnrolledIds.Contains(student.Id))
            {
                skippedCount++;
                continue;
            }

            newEnrollments.Add(new Enrollment
            {
                CourseId = courseId,
                StudentId = student.Id,
                Status = EnrollmentStatus.Active,
                Progress = 0,
                Created = DateTime.UtcNow,
                CreatedBy = currentUserId
            });
        }

        if (newEnrollments.Any())
        {
            await _dbContext.AddRangeAsync(newEnrollments, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return new BulkEnrollmentResult
        {
            Success = true,
            EnrolledCount = newEnrollments.Count,
            SkippedCount = skippedCount,
            Message = $"Успішно зараховано: {newEnrollments.Count}. Пропущено (вже були на курсі): {skippedCount}."
        };
    }
}
