using System.Linq;

namespace TaskManagement.Domain.ValueObjects;

public record Priority(string Name, int Order)
{
    public static Priority Low => new("Low", 0);
    public static Priority Medium => new("Medium", 1);
    public static Priority High => new("High", 2);
    public static Priority Critical => new("Critical", 3);

    public static IReadOnlyCollection<Priority> All => new[] { Low, Medium, High, Critical };

    public static Priority FromName(string name) =>
        All.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))
        ?? throw new ArgumentException($"Unsupported priority '{name}'", nameof(name));
}
