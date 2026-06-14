using PaStudy.Core.Helpers.DTOs.Category;
using PaStudy.Core.Helpers.DTOs.Course;
using PaStudy.Core.Helpers.DTOs.Course.Note;
using PaStudy.Core.Helpers.DTOs.Enrollment;
using PaStudy.Core.Helpers.Exceptions;
using PaStudy.Core.Helpers.FilterObjects;
using PaStudy.Core.Helpers.FilterObjects.CourseFilters;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PavStudy.API.Endpoints;

public class Courses : EndpointGroupBase
{

    public override void Map(WebApplication app)
    {
        app.MapGroup(this).RequireAuthorization()
            .MapGet(GetCourses)
            .MapGet(GetCourseById, "{id}")
            .MapGet(GetNotes, "{courseId}/notes")
            .MapGet(GetStudentsGradebook, "{courseId}/gradebook")
            .MapPost(EnrollToCourse, "{id}/enroll")
            .MapPost(CreateCourseAsync)
            .MapPost(BulkCreateCoursesAsync, "bulk-create")
            .MapGet(GetCategories, "Categories")
            .MapPut(UpdateCourse)
            .MapPost(BulkEnroll, "bulk-enroll");
    }

    public async Task<ImmutableArray<CourseDto>> GetCourses(CancellationToken cancellationToken, [AsParameters] CourseFilter courseFilter, ICourseRepository repository, ClaimsPrincipal user)
    {
        return await repository.GetCourses(cancellationToken, courseFilter, user);
    }

    public async Task<ImmutableArray<NoteDto>> GetNotes(CancellationToken cancellationToken, [AsParameters] BaseFilterRequest courseFilter, ICourseRepository repository, ClaimsPrincipal user, int courseId)
    {
        return await repository.GetNotesAsync(courseId, cancellationToken, user, courseFilter);
    }

    public async Task<IResult> GetStudentsGradebook(int courseId, CancellationToken ct, IEnrollmentRepository repository)
    {
        try
        {
            var gradebook = await repository.GetCourseGradebookAsync(courseId, ct);
            return Results.Ok(gradebook);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    public async Task<CourseDto> GetCourseById(int id, CancellationToken cancellationToken, ICourseRepository repository, ClaimsPrincipal user)
    {
        return await repository.GetCourseByIdAsync(id, cancellationToken, user);
    }


    public async Task<IResult> CreateCourseAsync(CreateCourseDto dto, CancellationToken cancellationToken, ICourseRepository repository, ClaimsPrincipal user)
    {
        try
        {
            var result = await repository.CreateCourseAsync(dto, user, cancellationToken);
            return result != null ? Results.Ok(result) : Results.BadRequest();
        }
        catch (ForbiddenException ex)
        {
            return Results.Problem(ex.Message, statusCode: 403);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    public async Task<IResult> BulkCreateCoursesAsync(List<CreateCourseDto> dtos, CancellationToken cancellationToken, ICourseRepository repository, ClaimsPrincipal user)
    {
        try
        {
            if (dtos == null || !dtos.Any())
            {
                return Results.BadRequest(new { message = "Список курсів не може бути порожнім." });
            }

            var createdCourses = new List<CourseResponseDto>();

            foreach (var dto in dtos)
            {
                var result = await repository.CreateCourseAsync(dto, user, cancellationToken);
                createdCourses.Add(result);
            }

            return Results.Ok(new
            {
                message = $"Successfully created {createdCourses.Count} courses.",
                courses = createdCourses
            });
        }
        catch (ForbiddenException ex)
        {
            return Results.Problem(ex.Message, statusCode: 403);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public async Task<IResult> EnrollToCourse(int id, CancellationToken cancellationToken, IEnrollmentRepository repository, ClaimsPrincipal user)
    {
        try
        {
            var result = await repository.CreateEnrollmentAsync(id, user, cancellationToken);
            return result ? Results.Ok(new { message = "Ви успішно зараховані на курс" }) : Results.BadRequest();
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(ex.Message, statusCode: 403);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }
    public async Task<IResult> UpdateCourse(UpdateCourseDto courseDto, CancellationToken ct, ICourseRepository repository, ClaimsPrincipal user)
    {
        try
        {
            await repository.UpdateCourseAsync(courseDto, user, ct);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
    public async Task<ImmutableArray<CategoryBriefDto>> GetCategories(CancellationToken cancellationToken, ICourseRepository repository)
    {
        return await repository.GetCategoryBriefInfo(cancellationToken);
    }
    public record BulkEnrollmentDto(int CourseId, List<string> Emails);

    public async Task<IResult> BulkEnroll(
        BulkEnrollmentDto dto,
        CancellationToken cancellationToken,
        IEnrollmentRepository repository,
        ClaimsPrincipal user)
    {
        try
        {
            if (dto.Emails == null || !dto.Emails.Any())
            {
                return Results.BadRequest(new { message = "Список імейлів не може бути порожнім." });
            }

            var result = await repository.BulkEnrollStudentsByEmailsAsync(dto.CourseId, dto.Emails, user, cancellationToken);

            return Results.Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Results.Unauthorized();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}