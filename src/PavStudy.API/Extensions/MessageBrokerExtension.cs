using MassTransit;

namespace PavStudy.API.Extensions;

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
