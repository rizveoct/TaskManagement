using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Email).IsRequired().HasMaxLength(256);
        builder.Property(user => user.FullName).IsRequired().HasMaxLength(200);
        builder.OwnsMany(user => user.Roles, roles =>
        {
            roles.ToTable("UserRoles");
            roles.WithOwner().HasForeignKey("UserId");
            roles.Property(role => role.Name).HasMaxLength(100);
        });
        builder.Navigation(user => user.Roles).Metadata.SetField("_roles");
        builder.Navigation(user => user.Teams).Metadata.SetField("_teams");
        builder.Navigation(user => user.Projects).Metadata.SetField("_projects");
    }
}
