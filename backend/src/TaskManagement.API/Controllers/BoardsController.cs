using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Boards.Commands;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/boards")]
[Authorize]
public class BoardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{boardId:guid}/statuses")]
    public async Task<IActionResult> UpdateStatuses(Guid boardId, [FromBody] UpdateBoardStatusesCommand command)
    {
        if (boardId != command.BoardId)
        {
            return BadRequest();
        }

        await _mediator.Send(command);
        return NoContent();
    }
}
