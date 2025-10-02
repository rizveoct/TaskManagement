using System.Linq;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.ValueObjects;
using TaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;


namespace TaskManagement.Application.Features.Boards.Commands;

public record UpdateBoardStatusesCommand(Guid BoardId, IEnumerable<string> Statuses) : IRequest;

public class UpdateBoardStatusesCommandHandler : IRequestHandler<UpdateBoardStatusesCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBoardStatusesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateBoardStatusesCommand request, CancellationToken cancellationToken)
    {
        var board = await _context.Boards
            .Include(b => b.Statuses)
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken)
            ?? throw new KeyNotFoundException($"Board {request.BoardId} was not found");

        var statuses = request.Statuses.Select(TaskStatus.FromName).ToList();
        board.ConfigureStatuses(statuses);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

public class UpdateBoardStatusesCommandValidator : AbstractValidator<UpdateBoardStatusesCommand>
{
    public UpdateBoardStatusesCommandValidator()
    {
        RuleFor(x => x.BoardId).NotEmpty();
        RuleFor(x => x.Statuses)
            .NotEmpty()
            .Must(statuses => statuses.Distinct(StringComparer.OrdinalIgnoreCase).Count() == statuses.Count())
            .WithMessage("Statuses must be unique");
    }
}
