using System.Linq;

namespace TaskManagement.Domain.ValueObjects;

public record TaskStatus(string Name, int Order)
{
    public static TaskStatus ToDo => new("To Do", 0);
    public static TaskStatus InProgress => new("In Progress", 1);
    public static TaskStatus Review => new("Review", 2);
    public static TaskStatus Done => new("Done", 3);

    public static IReadOnlyCollection<TaskStatus> DefaultStatuses => new[]
    {
        ToDo,
        InProgress,
        Review,
        Done
    };

    public static TaskStatus FromName(string name) =>
        DefaultStatuses.FirstOrDefault(status => string.Equals(status.Name, name, StringComparison.OrdinalIgnoreCase))
        ?? throw new ArgumentException($"Unsupported task status '{name}'", nameof(name));
}
