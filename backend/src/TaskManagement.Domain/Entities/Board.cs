using System.Linq;
using TaskManagement.Domain.Shared;
using TaskManagement.Domain.ValueObjects;
using TaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;


namespace TaskManagement.Domain.Entities;

public class Board : AuditableEntity
{
    private readonly List<TaskItem> _tasks = new();
    private readonly List<TaskStatus> _statuses = new();

    private Board()
    {
    }

    public Board(string name, Guid projectId, IEnumerable<TaskStatus> statuses)
    {
        Name = name;
        ProjectId = projectId;
        _statuses.AddRange(statuses);
    }

    public string Name { get; private set; } = string.Empty;
    public Guid ProjectId { get; private set; }
    public bool IsArchived { get; private set; }
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();
    public IReadOnlyCollection<TaskStatus> Statuses => _statuses.AsReadOnly();

    public void Rename(string name) => Name = name;

    public void Archive() => IsArchived = true;

    public void Restore() => IsArchived = false;

    public TaskItem CreateTask(
        string title,
        string description,
        Priority priority,
        TaskStatus status,
        DateTime? dueDate)
    {
        if (!_statuses.Any(s => s.Name == status.Name))
        {
            throw new InvalidOperationException($"Status {status.Name} is not enabled for this board");
        }

        var task = new TaskItem(title, description, priority, status, Id)
        {
            DueDateUtc = dueDate
        };

        _tasks.Add(task);
        return task;
    }

    public void ConfigureStatuses(IEnumerable<TaskStatus> statuses)
    {
        _statuses.Clear();
        _statuses.AddRange(statuses);
    }
}
