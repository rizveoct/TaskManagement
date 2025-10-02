using System.Linq;
using TaskManagement.Domain.Events;
using TaskManagement.Domain.Shared;
using TaskManagement.Domain.ValueObjects;
using TaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;


namespace TaskManagement.Domain.Entities;

public class TaskItem : AuditableEntity
{
    private readonly List<SubTask> _subTasks = new();
    private readonly List<TaskComment> _comments = new();
    private readonly List<TaskAssignment> _assignees = new();
    private readonly List<TaskTag> _tags = new();
    private readonly List<TaskAttachment> _attachments = new();

    private TaskItem()
    {
    }

    public TaskItem(string title, string description, Priority priority, TaskStatus status, Guid boardId)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Status = status;
        BoardId = boardId;
    }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Priority Priority { get; private set; } = Priority.Medium;
    public TaskStatus Status { get; private set; } = TaskStatus.ToDo;
    public Guid BoardId { get; private set; }
    public DateTime? DueDateUtc { get; set; }
    public DateTime? StartDateUtc { get; set; }
    public TimeSpan? Estimate { get; set; }

    public IReadOnlyCollection<SubTask> SubTasks => _subTasks.AsReadOnly();
    public IReadOnlyCollection<TaskComment> Comments => _comments.AsReadOnly();
    public IReadOnlyCollection<TaskAssignment> Assignees => _assignees.AsReadOnly();
    public IReadOnlyCollection<TaskTag> Tags => _tags.AsReadOnly();
    public IReadOnlyCollection<TaskAttachment> Attachments => _attachments.AsReadOnly();

    public void UpdateDetails(string title, string description, Priority priority, TaskStatus status)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Status = status;
    }

    public void AssignUser(Guid userId)
    {
        if (_assignees.Any(a => a.UserId == userId))
        {
            return;
        }

        _assignees.Add(new TaskAssignment(Id, userId));
        RaiseDomainEvent(new TaskAssignedEvent(Id, userId, DateTime.UtcNow));
    }

    public void RemoveUser(Guid userId)
    {
        _assignees.RemoveAll(a => a.UserId == userId);
    }

    public SubTask AddSubTask(string title)
    {
        var subTask = new SubTask(title);
        _subTasks.Add(subTask);
        return subTask;
    }

    public TaskComment AddComment(Guid userId, string body)
    {
        var comment = new TaskComment(userId, body);
        _comments.Add(comment);
        return comment;
    }

    public TaskTag AddTag(string name)
    {
        var tag = new TaskTag(name);
        if (_tags.Contains(tag))
        {
            return tag;
        }

        _tags.Add(tag);
        return tag;
    }

    public TaskAttachment AddAttachment(string fileName, string contentType, long size, string url)
    {
        var attachment = new TaskAttachment(fileName, contentType, size, url);
        _attachments.Add(attachment);
        return attachment;
    }
}

public record TaskAssignment(Guid TaskId, Guid UserId);

public record TaskTag(string Name)
{
    public string NormalizedName => Name.ToLowerInvariant();
}

public record SubTask(string Title)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public bool IsCompleted { get; private set; }

    public void Toggle(bool completed) => IsCompleted = completed;
}

public record TaskAttachment(string FileName, string ContentType, long SizeBytes, string Url)
{
    public Guid Id { get; init; } = Guid.NewGuid();
}

public record TaskComment(Guid UserId, string Body)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
};
