using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Common.Interfaces;

public interface INotificationService
{
    Task PublishAsync(Notification notification, CancellationToken cancellationToken = default);
}
