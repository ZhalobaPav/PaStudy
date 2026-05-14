using CloudinaryDotNet;
using MassTransit;
using PaStudy.NotificationService;
using PaStudy.NotificationService.Consumers;
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Твої стандартні налаштування валідації токена (Key, Issuer тощо)...

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Перехоплюємо токен з query-параметра, якщо запит йде до нашого SignalR хабу
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/");
        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        cfg.ConfigureEndpoints(context);
    });
});
var host = builder.Build();
app.MapHub<NotificationHub>("/hubs/notifications");
host.Run();
