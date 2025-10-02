using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Application.Features.Projects.Queries;

public record GetProjectDashboardQuery(Guid ProjectId) : IRequest<ProjectDashboardDto>;

public class GetProjectDashboardQueryHandler : IRequestHandler<GetProjectDashboardQuery, ProjectDashboardDto>
{
    private readonly IApplicationDbContext _context;

    public GetProjectDashboardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectDashboardDto> Handle(GetProjectDashboardQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Boards)
                .ThenInclude(b => b.Tasks)
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken)
            ?? throw new KeyNotFoundException($"Project {request.ProjectId} was not found");

        var tasks = project.Boards.SelectMany(b => b.Tasks).ToList();
        var total = tasks.Count;
        var completed = tasks.Count(t => t.Status.Name.Equals("Done", StringComparison.OrdinalIgnoreCase));

        return new ProjectDashboardDto
        {
            ProjectId = project.Id,
            ProjectName = project.Name,
            TotalTasks = total,
            CompletedTasks = completed,
            CompletionRate = total == 0 ? 0 : completed / (double)total,
            Boards = project.Boards.Select(board => new BoardSummaryDto
            {
                BoardId = board.Id,
                Name = board.Name,
                TaskCount = board.Tasks.Count,
                ActiveStatuses = board.Statuses.Select(status => status.Name).ToArray()
            }).ToArray()
        };
    }
}

public record ProjectDashboardDto
{
    public Guid ProjectId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public int TotalTasks { get; init; }
    public int CompletedTasks { get; init; }
    public double CompletionRate { get; init; }
    public IReadOnlyCollection<BoardSummaryDto> Boards { get; init; } = Array.Empty<BoardSummaryDto>();
}

public record BoardSummaryDto
{
    public Guid BoardId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int TaskCount { get; init; }
    public IReadOnlyCollection<string> ActiveStatuses { get; init; } = Array.Empty<string>();
}
