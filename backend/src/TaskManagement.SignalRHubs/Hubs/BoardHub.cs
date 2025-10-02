using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.SignalRHubs.Hubs;

[Authorize]
public class BoardHub : Hub
{
    public async Task BroadcastBoardUpdate(Guid projectId, Guid boardId)
    {
        await Clients.Group(GetProjectGroup(projectId)).SendAsync("BoardUpdated", boardId);
    }

    public async Task JoinProject(Guid projectId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetProjectGroup(projectId));
        await Clients.Caller.SendAsync("JoinedProject", projectId);
    }

    public async Task LeaveProject(Guid projectId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetProjectGroup(projectId));
        await Clients.Caller.SendAsync("LeftProject", projectId);
    }

    private static string GetProjectGroup(Guid projectId) => $"project-{projectId}";
}
