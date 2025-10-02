using System.Linq;
using TaskManagement.Domain.Shared;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Entities;

public class User : AuditableEntity
{
    private readonly List<UserRole> _roles = new();
    private readonly List<Team> _teams = new();
    private readonly List<ProjectMembership> _projects = new();

    private User()
    {
    }

    public User(string email, string fullName)
    {
        Email = email;
        FullName = fullName;
        _roles.Add(UserRole.Member);
    }

    public string Email { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public bool EmailConfirmed { get; private set; }
    public string? AvatarUrl { get; private set; }
    public DateTime? LastLoginUtc { get; private set; }

    public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<Team> Teams => _teams.AsReadOnly();
    public IReadOnlyCollection<ProjectMembership> Projects => _projects.AsReadOnly();

    public void ConfirmEmail() => EmailConfirmed = true;

    public void UpdateProfile(string fullName, string? avatarUrl)
    {
        FullName = fullName;
        AvatarUrl = avatarUrl;
    }

    public void RecordLogin() => LastLoginUtc = DateTime.UtcNow;

    public void AddRole(UserRole role)
    {
        if (_roles.Any(r => r.Name == role.Name))
        {
            return;
        }

        _roles.Add(role);
    }

    public void RemoveRole(UserRole role)
    {
        _roles.RemoveAll(r => r.Name == role.Name);
    }

    public void JoinTeam(Team team)
    {
        if (_teams.Contains(team))
        {
            return;
        }

        _teams.Add(team);
    }

    public void AssignToProject(Project project, UserRole role)
    {
        if (_projects.Any(m => m.ProjectId == project.Id))
        {
            return;
        }

        _projects.Add(new ProjectMembership(project.Id, Id, role));
    }
}

public record ProjectMembership(Guid ProjectId, Guid UserId, UserRole Role);
