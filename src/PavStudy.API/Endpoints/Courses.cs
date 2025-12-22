using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Course;
using PaStudy.Core.Helpers.FilterObjects.CourseFilters;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.ConfigureDependencies;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Collections.Immutable;

namespace PavStudy.API.Endpoints;

public class Courses : EndpointGroupBase
{

    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetCourses)
            .MapGet(GetCourseById, "{id}");
    }

    public async Task<ImmutableArray<CourseDto>> GetCourses(CancellationToken cancellationToken,[AsParameters] CourseFilter courseFilter, ICourseRepository repository)
    {
        return await repository.GetCourses(cancellationToken, courseFilter);
    }

    public async Task<CourseDto> GetCourseById(int id, CancellationToken cancellationToken, ICourseRepository repository)
    {
        return await repository.GetCourseByIdAsync(id, cancellationToken);
    }
}
