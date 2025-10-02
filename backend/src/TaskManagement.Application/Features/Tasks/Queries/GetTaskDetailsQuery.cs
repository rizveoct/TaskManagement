using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Features.Tasks.Commands;

namespace TaskManagement.Application.Features.Tasks.Queries;

public record GetTaskDetailsQuery(Guid TaskId) : IRequest<TaskDetailsDto>;

public class GetTaskDetailsQueryHandler : IRequestHandler<GetTaskDetailsQuery, TaskDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public GetTaskDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskDetailsDto> Handle(GetTaskDetailsQuery request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .Where(t => t.Id == request.TaskId)
            .Select(task => new TaskDetailsDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority.Name,
                Status = task.Status.Name,
                DueDate = task.DueDateUtc,
                AssigneeIds = task.Assignees.Select(assignee => assignee.UserId).ToArray(),
                BoardName = _context.Boards.Where(board => board.Id == task.BoardId).Select(board => board.Name).FirstOrDefault() ?? string.Empty,
                Comments = task.Comments.Select(comment => new TaskCommentDto(comment.Id, comment.UserId, comment.Body, comment.CreatedAtUtc)).ToArray(),
                SubTasks = task.SubTasks.Select(subtask => new SubTaskDto(subtask.Id, subtask.Title, subtask.IsCompleted)).ToArray(),
                Attachments = task.Attachments.Select(attachment => new TaskAttachmentDto(attachment.Id, attachment.FileName, attachment.ContentType, attachment.SizeBytes, attachment.Url)).ToArray(),
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException($"Task {request.TaskId} was not found");

        return task;
    }
}

public record TaskDetailsDto : TaskDto
{
    public string BoardName { get; init; } = string.Empty;
    public IReadOnlyCollection<TaskCommentDto> Comments { get; init; } = Array.Empty<TaskCommentDto>();
    public IReadOnlyCollection<SubTaskDto> SubTasks { get; init; } = Array.Empty<SubTaskDto>();
    public IReadOnlyCollection<TaskAttachmentDto> Attachments { get; init; } = Array.Empty<TaskAttachmentDto>();
}

public record TaskCommentDto(Guid Id, Guid UserId, string Body, DateTime CreatedAtUtc);

public record SubTaskDto(Guid Id, string Title, bool IsCompleted);

public record TaskAttachmentDto(Guid Id, string FileName, string ContentType, long SizeBytes, string Url);
