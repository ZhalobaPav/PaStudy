using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace PaStudy.Infrastructure.ConfigureDependencies;

public static class MessageBrokerExtension
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/");
            });
        });

        return services;
    }
}
