using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Project> Projects { get; }
    DbSet<Board> Boards { get; }
    DbSet<TaskItem> Tasks { get; }
    DbSet<TaskComment> TaskComments { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Team> Teams { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
