using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PaStudy.Core.Interfaces.Repository;

namespace PaStudy.NotificationService;
[Authorize]
public class NotificationHub: Hub
{
    private readonly ICourseRepository _courseRepository;

    public NotificationHub(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;

        if (!string.IsNullOrEmpty(userId))
        {
            var courseIds = await _courseRepository.GetUserCourseIdsAsync(userId);

            foreach (var courseId in courseIds)
            {
                var groupName = $"Course_{courseId}";
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;

        if (!string.IsNullOrEmpty(userId))
        {
            var courseIds = await _courseRepository.GetUserCourseIdsAsync(userId);

            foreach (var courseId in courseIds)
            {
                var groupName = $"Course_{courseId}";
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}
