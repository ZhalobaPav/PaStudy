using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaStudy.Infrastructure.Data;
using System.Net;

namespace PaStudy.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static void ApplyDockerMigrations(this IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

        if (!configuration.GetValue<bool>("RUN_MIGRATIONS"))
        {
            return;
        }

        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<PaStudyDbContext>>();

        try
        {
            logger.LogInformation("Початок автоматичного застосування міграцій у Docker (через Infrastructure)...");

            var context = services.GetRequiredService<PaStudyDbContext>();

            // Отримуємо рядок підключення
            var connectionString = configuration.GetConnectionString("PaStudyConnection");

            // Створюємо підключення до системної бази 'master', щоб перевірити чи живий САМ СЕРВЕР
            var masterBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master" // Тимчасово стукаємо в гарантовано існуючу базу master
            };

            var serverReady = false;

            for (int i = 0; i < 6; i++)
            {
                try
                {
                    using var conn = new SqlConnection(masterBuilder.ConnectionString);
                    conn.Open(); // Намагаємося відкрити з'єднання з сервером
                    serverReady = true;
                    break;
                }
                catch (SqlException)
                {
                    logger.LogWarning("SQL Сервер ще не приймає підключення, чекаємо 5 секунд... (Спроба {Attempt}/6)", i + 1);
                    Thread.Sleep(5000);
                }
            }

            if (serverReady)
            {
                logger.LogInformation("SQL Сервер готовий! Запускаємо міграції та створення БД...");
                context.Database.Migrate(); // Створить базу PaStudyDb та накотить міграції
                logger.LogInformation("Міграції успішно застосовані!");
            }
            else
            {
                logger.LogError("Не вдалося підключитися до SQL Сервера за 30 секунд. Міграція скасована.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Критична помилка під час виконання міграцій в шарі Infrastructure.");
        }
    }
}