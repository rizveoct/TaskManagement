using System.Linq;
using TaskManagement.Domain.Shared;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Entities;

public class Project : AuditableEntity
{
    private readonly List<Board> _boards = new();
    private readonly List<ProjectMembership> _members = new();

    private Project()
    {
    }

    public Project(string name, Guid organizationId)
    {
        Name = name;
        OrganizationId = organizationId;
    }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid OrganizationId { get; private set; }
    public DateTime? StartDateUtc { get; private set; }
    public DateTime? DueDateUtc { get; private set; }

    public IReadOnlyCollection<Board> Boards => _boards.AsReadOnly();
    public IReadOnlyCollection<ProjectMembership> Members => _members.AsReadOnly();

    public void UpdateDetails(string name, string? description, DateTime? startDate, DateTime? dueDate)
    {
        Name = name;
        Description = description;
        StartDateUtc = startDate;
        DueDateUtc = dueDate;
    }

    public Board CreateBoard(string name, IEnumerable<TaskStatus>? statuses = null)
    {
        var board = new Board(name, Id, statuses ?? TaskStatus.DefaultStatuses);
        _boards.Add(board);
        return board;
    }

    public void AddMember(User user, UserRole role)
    {
        if (_members.Any(m => m.UserId == user.Id))
        {
            return;
        }

        var membership = new ProjectMembership(Id, user.Id, role);
        _members.Add(membership);
        user.AssignToProject(this, role);
    }
}
