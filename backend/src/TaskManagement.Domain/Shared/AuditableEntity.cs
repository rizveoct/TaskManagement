namespace TaskManagement.Domain.Shared;

public abstract class AuditableEntity : Entity
{
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime? LastModifiedAtUtc { get; private set; }
    public string? LastModifiedBy { get; private set; }
    public DateTime? DeletedAtUtc { get; private set; }
    public bool IsDeleted => DeletedAtUtc.HasValue;

    public void MarkCreated(string userId)
    {
        CreatedBy = userId;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void MarkModified(string userId)
    {
        LastModifiedBy = userId;
        LastModifiedAtUtc = DateTime.UtcNow;
    }

    public void SoftDelete(string userId)
    {
        if (IsDeleted)
        {
            return;
        }

        DeletedAtUtc = DateTime.UtcNow;
        LastModifiedBy = userId;
        LastModifiedAtUtc = DeletedAtUtc;
    }
}
