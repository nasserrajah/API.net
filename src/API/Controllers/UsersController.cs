using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using core_first.Application.Features.Users.DTOs;
using core_first.Application.Features.Users.Queries.GetAll;
using core_first.Application.Features.Users.Queries.GetById;
using core_first.Application.Features.Users.Commands.UpdateRole;
using core_first.Application.Features.Users.Commands.Delete;
using core_first.Domain.Enums;

namespace core_first.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllUsersQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetUserByIdQuery { Id = id };
        var user = await _mediator.Send(query);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateRole(int id, [FromQuery] UserRole role)
    {
        var command = new UpdateUserRoleCommand { Id = id, NewRole = role };
        var success = await _mediator.Send(command);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteUserCommand { Id = id };
        var success = await _mediator.Send(command);
        if (!success) return NotFound();
        return NoContent();
    }
}