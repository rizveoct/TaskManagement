using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.SignalRHubs.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (Context.UserIdentifier is { } userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetUserGroup(userId));
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.UserIdentifier is { } userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetUserGroup(userId));
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task PublishNotification(Guid notificationId, string message)
    {
        if (Context.UserIdentifier is not { } userId)
        {
            return;
        }

        await Clients.Group(GetUserGroup(userId)).SendAsync("NotificationReceived", notificationId, message);
    }

    private static string GetUserGroup(string userId) => $"user-{userId}";
}
