using Microsoft.Extensions.DependencyInjection;
using PaStudy.Core.Interfaces;
using PaStudy.Infrastructure.Repositories;
using PaStudy.Infrastructure.Services;

namespace PaStudy.Infrastructure.ConfigureDependencies;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<DataSeederService>();
        return services;
    }
}
