using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Projects.Queries;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}/dashboard")]
    public async Task<ActionResult<ProjectDashboardDto>> GetDashboard(Guid id)
    {
        var result = await _mediator.Send(new GetProjectDashboardQuery(id));
        return Ok(result);
    }
}
