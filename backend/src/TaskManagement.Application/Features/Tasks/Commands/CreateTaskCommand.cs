using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.ValueObjects;
using TaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;

namespace TaskManagement.Application.Features.Tasks.Commands;

public record CreateTaskCommand(
    Guid BoardId,
    string Title,
    string Description,
    string Priority,
    string Status,
    DateTime? DueDate,
    IEnumerable<Guid> AssigneeIds)
    : IRequest<TaskDto>;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var board = await _context.Boards
            .Include(b => b.Tasks)
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException($"Board {request.BoardId} was not found");

        var task = board.CreateTask(
            request.Title,
            request.Description,
            Priority.FromName(request.Priority),
            TaskStatus.FromName(request.Status),
            request.DueDate);

        foreach (var userId in request.AssigneeIds.Distinct())
        {
            task.AssignUser(userId);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TaskDto>(task);
    }
}

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.BoardId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Priority).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}

public record TaskDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime? DueDate { get; init; }
    public IReadOnlyCollection<Guid> AssigneeIds { get; init; } = Array.Empty<Guid>();
}
