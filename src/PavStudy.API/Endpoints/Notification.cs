using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;

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
}
