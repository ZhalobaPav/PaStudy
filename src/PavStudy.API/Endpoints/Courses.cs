using PaStudy.Core.Entities;
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
            .MapGet(GetCourses);
    }

    public async Task<ImmutableArray<Course>> GetCourses(CancellationToken cancellationToken, ICourseRepository repository)
    {
        return await repository.GetCourses(cancellationToken);
    }
}
