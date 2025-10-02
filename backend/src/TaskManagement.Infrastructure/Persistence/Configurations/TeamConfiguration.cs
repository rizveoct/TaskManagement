using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");
        builder.HasKey(team => team.Id);
        builder.Property(team => team.Name).IsRequired().HasMaxLength(200);
        builder.Property(team => team.OrganizationId).IsRequired();
        builder.HasMany(team => team.Members)
            .WithMany(user => user.Teams)
            .UsingEntity(join => join.ToTable("TeamMembers"));
    }
}
