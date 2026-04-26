using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaStudy.Core.Helpers.ConfigurationModels;

namespace PaStudy.Infrastructure.ConfigureDependencies.BindConfigurations;

public static class CloudinaryConfiguration
{
    public static IServiceCollection BindCloudinaryConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(
            configuration.GetSection("CloudinarySettings")
        );

        return services;
    }
}
