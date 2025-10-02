using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.SignalRHubs.Hubs;

[Authorize]
public class TaskHub : Hub
{
    public async Task JoinBoardGroup(Guid boardId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetBoardGroup(boardId));
        await Clients.Caller.SendAsync("JoinedBoard", boardId);
    }

    public async Task LeaveBoardGroup(Guid boardId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetBoardGroup(boardId));
        await Clients.Caller.SendAsync("LeftBoard", boardId);
    }

    public async Task NotifyTaskUpdated(Guid boardId, Guid taskId)
    {
        await Clients.Group(GetBoardGroup(boardId)).SendAsync("TaskUpdated", taskId);
    }

    public async Task Typing(Guid boardId, Guid taskId, string userName)
    {
        await Clients.OthersInGroup(GetBoardGroup(boardId)).SendAsync("Typing", taskId, userName);
    }

    private static string GetBoardGroup(Guid boardId) => $"board-{boardId}";
}
