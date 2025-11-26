using PaStudy.Core.Entities;
using PaStudy.Core.Interfaces;
using PaStudy.Infrastructure.ConfigureDependencies;
using PaStudy.Infrastructure.Models;
using System.Collections.Immutable;

namespace PavStudy.API.Endpoints;

public class Courses : EndpointGroupBase
{
    private readonly ICourseRepository _repository;

    public Courses(ICourseRepository repository)
    {
        _repository = repository;
    }
    public override void Map(WebApplication app)
    {
        app.MapGroupd(this)
            .MapGet("/", () => "This is the Courses endpoint.");
    }

    public async Task<ImmutableArray<Course>> GetCourses(CancellationToken cancellationToken)
    {
        return await _repository.GetCourses(cancellationToken);
    }
}
