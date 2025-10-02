namespace TaskManagement.Domain.Entities;

using TaskManagement.Domain.Shared;

public class Team : AuditableEntity
{
    private readonly List<User> _members = new();

    private Team()
    {
    }

    public Team(string name, Guid organizationId)
    {
        Name = name;
        OrganizationId = organizationId;
    }

    public string Name { get; private set; } = string.Empty;
    public Guid OrganizationId { get; private set; }

    public IReadOnlyCollection<User> Members => _members.AsReadOnly();

    public void Rename(string name)
    {
        Name = name;
    }

    public void AddMember(User user)
    {
        if (_members.Contains(user))
        {
            return;
        }

        _members.Add(user);
        user.JoinTeam(this);
    }

    public void RemoveMember(User user)
    {
        _members.Remove(user);
    }
}
