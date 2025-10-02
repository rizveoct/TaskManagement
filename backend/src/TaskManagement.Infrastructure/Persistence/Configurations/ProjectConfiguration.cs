using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(project => project.Id);
        builder.Property(project => project.Name).IsRequired().HasMaxLength(200);
        builder.Property(project => project.OrganizationId).IsRequired();
        builder.HasMany(project => project.Boards)
            .WithOne()
            .HasForeignKey(board => board.ProjectId);
        builder.Navigation(project => project.Boards).Metadata.SetField("_boards");
        builder.Navigation(project => project.Members).Metadata.SetField("_members");
        builder.OwnsMany(project => project.Members, membership =>
        {
            membership.ToTable("ProjectMembers");
            membership.WithOwner().HasForeignKey("ProjectId");
            membership.Property(x => x.UserId).IsRequired();
            membership.Property(x => x.Role)
                .HasConversion(role => role.Name, value => TaskManagement.Domain.ValueObjects.UserRole.FromName(value))
                .HasMaxLength(100);
        });
    }
}
