using TaskManagement.Domain.Shared;

namespace TaskManagement.Domain.Entities;

public class Notification : AuditableEntity
{
    private Notification()
    {
    }

    public Notification(Guid userId, string message, string category)
    {
        UserId = userId;
        Message = message;
        Category = category;
    }

    public Guid UserId { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }
    public string? ActionUrl { get; private set; }

    public void MarkAsRead() => IsRead = true;

    public void AttachAction(string url) => ActionUrl = url;
}
