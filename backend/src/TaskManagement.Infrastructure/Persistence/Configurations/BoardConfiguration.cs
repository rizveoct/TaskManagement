using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

public class BoardConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.ToTable("Boards");
        builder.HasKey(board => board.Id);
        builder.Property(board => board.Name).IsRequired().HasMaxLength(200);
        builder.Property(board => board.ProjectId).IsRequired();
        builder.HasMany(board => board.Tasks)
            .WithOne()
            .HasForeignKey(task => task.BoardId);
        builder.Navigation(board => board.Tasks).Metadata.SetField("_tasks");
        builder.Navigation(board => board.Statuses).Metadata.SetField("_statuses");
        builder.OwnsMany(board => board.Statuses, statuses =>
        {
            statuses.ToTable("BoardStatuses");
            statuses.WithOwner().HasForeignKey("BoardId");
            statuses.Property(status => status.Name).HasMaxLength(100);
        });
    }
}
