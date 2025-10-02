using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace TaskManagement.SignalRHubs.Hubs;

[Authorize]
public class PresenceHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> OnlineUsers = new();

    public override async Task OnConnectedAsync()
    {
        if (Context.UserIdentifier is { } userId)
        {
            OnlineUsers[Context.ConnectionId] = userId;
            await Clients.All.SendAsync("UserOnline", userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (OnlineUsers.TryRemove(Context.ConnectionId, out var userId))
        {
            await Clients.All.SendAsync("UserOffline", userId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public Task<IReadOnlyCollection<string>> GetOnlineUsers()
    {
        var users = OnlineUsers.Values.Distinct().ToArray();
        return Task.FromResult((IReadOnlyCollection<string>)users);
    }
}
