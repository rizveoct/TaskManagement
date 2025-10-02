using System.Linq;

namespace TaskManagement.Domain.ValueObjects;

public record UserRole(string Name, int Hierarchy)
{
    public static UserRole SuperAdmin => new("Super Admin", 0);
    public static UserRole Admin => new("Admin", 1);
    public static UserRole ProjectManager => new("Project Manager", 2);
    public static UserRole TeamLead => new("Team Lead", 3);
    public static UserRole Member => new("Member", 4);
    public static UserRole Guest => new("Guest", 5);

    public static IReadOnlyCollection<UserRole> All => new[]
    {
        SuperAdmin,
        Admin,
        ProjectManager,
        TeamLead,
        Member,
        Guest
    };

    public static UserRole FromName(string name) =>
        All.FirstOrDefault(role => string.Equals(role.Name, name, StringComparison.OrdinalIgnoreCase))
        ?? throw new ArgumentException($"Unsupported user role '{name}'", nameof(name));
}
