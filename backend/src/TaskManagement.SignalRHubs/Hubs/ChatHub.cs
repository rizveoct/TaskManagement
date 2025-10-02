using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.SignalRHubs.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendMessage(Guid taskId, string message)
    {
        await Clients.Group(GetTaskGroup(taskId)).SendAsync("ReceiveMessage", Context.User?.Identity?.Name, message, DateTime.UtcNow);
    }

    public async Task JoinTask(Guid taskId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetTaskGroup(taskId));
    }

    public async Task LeaveTask(Guid taskId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetTaskGroup(taskId));
    }

    private static string GetTaskGroup(Guid taskId) => $"task-chat-{taskId}";
}
