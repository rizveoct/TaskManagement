using MediatR;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public PerformanceBehavior(
        ILogger<TRequest> logger,
        ICurrentUserService currentUserService,
        IDateTime dateTime)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var start = _dateTime.UtcNow;
        var response = await next();
        var elapsed = _dateTime.UtcNow - start;

        if (elapsed > TimeSpan.FromMilliseconds(500))
        {
            _logger.LogWarning(
                "Long running request: {Name} ({Elapsed} ms) {@User} {@Request}",
                typeof(TRequest).Name,
                elapsed.TotalMilliseconds,
                _currentUserService.UserId,
                request);
        }

        return response;
    }
}
