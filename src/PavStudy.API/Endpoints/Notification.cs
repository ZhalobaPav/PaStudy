using Microsoft.AspNetCore.Mvc;
using PaStudy.Core.Entities.Notification;
using PaStudy.Core.Helpers.DTOs.Notification;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;
using System.Security.Claims;

namespace PavStudy.API.Endpoints;

public class Notification : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).RequireAuthorization().MapGet(GetNotifciationsAsync);

    }

    public async Task<IResult> GetNotifciationsAsync(CancellationToken ct, INotificationRepository repository)
    {
        try
        {
            var notifications = await repository.GetNotifications(ct);
            return Results.Ok(notifications);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
    public async Task<IResult> SendCourseInviteAsync(
        [FromBody] SendInviteDto dto,
        INotificationRepository notificationRepo,
        CancellationToken ct)
    {
        try
        {
            var notificationDto = new CreateNotificationDto
            {
                Title = "Запрошення на курс",
                Message = $"Вас запрошено приєднатися до курсу: {dto.CourseName}. Бажаєте прийняти запрошення?",
                Type = NotificationType.CourseInvitation,
                RecipientUserId = dto.RecipientUserId,
                CourseId = dto.CourseId,
                IsRead = false
            };

            await notificationRepo.AddNotificationAsync(notificationDto);
            return Results.Ok(new { message = "Запрошення успішно надіслано студенту." });
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public async Task<IResult> RespondToInviteAsync(
        int id,
        [FromBody] RespondToInviteDto dto,
        INotificationRepository notificationRepo,
        IEnrollmentRepository enrollmentRepo, // Інжектуємо твій репозиторій зарахування
        ClaimsPrincipal user,
        CancellationToken ct)
    {
        try
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Results.Unauthorized();

            // 3.1. Оновлюємо статус нотифікації
            var (success, courseId) = await notificationRepo.RespondToInvitationAsync(id, userId, dto.Accept, ct);

            if (!success || courseId == null)
            {
                return Results.BadRequest(new { message = "Запрошення не знайдено, або воно вже оброблене." });
            }

            // 3.2. Якщо студент натиснув "Прийняти" (Accept == true)
            if (dto.Accept)
            {
                // Викликаємо твій готовий метод зарахування
                var enrolled = await enrollmentRepo.CreateEnrollmentAsync(courseId.Value, user, ct);

                if (enrolled)
                {
                    return Results.Ok(new { message = "Ви успішно приєдналися до курсу!" });
                }
                else
                {
                    return Results.BadRequest(new { message = "Не вдалося зарахувати на курс." });
                }
            }

            // Якщо відхилив
            return Results.Ok(new { message = "Ви відхилили запрошення." });
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(ex.Message, statusCode: 403);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
