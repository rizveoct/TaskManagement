using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Dispatching notification {NotificationId} for user {UserId} in category {Category}",
            notification.Id,
            notification.UserId,
            notification.Category);

        return Task.CompletedTask;
    }
}
