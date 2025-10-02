using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");
        builder.HasKey(task => task.Id);
        builder.Property(task => task.Title).IsRequired().HasMaxLength(200);
        builder.Property(task => task.Description).IsRequired();
        builder.Property(task => task.BoardId).IsRequired();

        builder.OwnsMany(task => task.SubTasks, subTask =>
        {
            subTask.ToTable("SubTasks");
            subTask.WithOwner().HasForeignKey("TaskId");
            subTask.Property(x => x.Title).HasMaxLength(200);
        });

        builder.OwnsMany(task => task.Attachments, attachment =>
        {
            attachment.ToTable("Attachments");
            attachment.WithOwner().HasForeignKey("TaskId");
            attachment.Property(x => x.FileName).HasMaxLength(260);
        });

        builder.OwnsMany(task => task.Tags, tag =>
        {
            tag.ToTable("TaskTags");
            tag.WithOwner().HasForeignKey("TaskId");
            tag.Property(x => x.Name).HasMaxLength(100);
        });

        builder.OwnsMany(task => task.Assignees, assignee =>
        {
            assignee.ToTable("TaskAssignees");
            assignee.WithOwner().HasForeignKey("TaskId");
        });

        builder.OwnsMany(task => task.Comments, comment =>
        {
            comment.ToTable("Comments");
            comment.WithOwner().HasForeignKey("TaskId");
            comment.Property(x => x.Body).IsRequired();
        });
    }
}
