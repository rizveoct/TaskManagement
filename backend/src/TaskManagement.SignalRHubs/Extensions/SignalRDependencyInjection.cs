using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.SignalRHubs.Hubs;

namespace TaskManagement.SignalRHubs;

public static class SignalRDependencyInjection
{
    public static IServiceCollection AddSignalRHubs(this IServiceCollection services)
    {
        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaximumReceiveMessageSize = 1024 * 1024;
        });

        return services;
    }

    public static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHub<TaskHub>("/hubs/tasks");
        app.MapHub<BoardHub>("/hubs/boards");
        app.MapHub<NotificationHub>("/hubs/notifications");
        app.MapHub<ChatHub>("/hubs/chat");
        app.MapHub<PresenceHub>("/hubs/presence");
        return app;
    }
}
