using PaStudy.Core.Helpers.DTOs.Category;
using PaStudy.Core.Helpers.DTOs.Course;
using PaStudy.Core.Helpers.DTOs.Course.Note;
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
            .MapPost(EnrollToCourse, "{id}/enroll")
            .MapPost(CreateCourseAsync)
            .MapGet(GetCategories, "Categories");
    }

    public async Task<ImmutableArray<CourseDto>> GetCourses(CancellationToken cancellationToken, [AsParameters] CourseFilter courseFilter, ICourseRepository repository, ClaimsPrincipal user)
    {
        return await repository.GetCourses(cancellationToken, courseFilter, user);
    }

    public async Task<ImmutableArray<NoteDto>> GetNotes(CancellationToken cancellationToken, [AsParameters] BaseFilterRequest courseFilter, ICourseRepository repository, ClaimsPrincipal user, int courseId)
    {
        return await repository.GetNotesAsync(courseId, cancellationToken, user, courseFilter);
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
        catch(ForbiddenException ex)
        {
            return Results.Problem(ex.Message, statusCode: 403);
        }
        catch(Exception ex)
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

    public async Task<ImmutableArray<CategoryBriefDto>> GetCategories(CancellationToken cancellationToken, ICourseRepository repository)
    {
        return await repository.GetCategoryBriefInfo(cancellationToken);
    }
}