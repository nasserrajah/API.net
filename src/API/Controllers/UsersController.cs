using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using core_first.Application.Features.Users.DTOs;
using core_first.Application.Features.Users.Interfaces;
using core_first.Domain.Enums;

namespace core_first.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IGetAllUsersQuery _getAllUsersQuery;
    private readonly IGetUserByIdQuery _getUserByIdQuery;
    private readonly IUpdateUserRoleCommand _updateUserRoleCommand;
    private readonly IDeleteUserCommand _deleteUserCommand;

    public UsersController(
        IGetAllUsersQuery getAllUsersQuery,
        IGetUserByIdQuery getUserByIdQuery,
        IUpdateUserRoleCommand updateUserRoleCommand,
        IDeleteUserCommand deleteUserCommand)
    {
        _getAllUsersQuery = getAllUsersQuery;
        _getUserByIdQuery = getUserByIdQuery;
        _updateUserRoleCommand = updateUserRoleCommand;
        _deleteUserCommand = deleteUserCommand;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _getAllUsersQuery.ExecuteAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _getUserByIdQuery.ExecuteAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateRole(int id, [FromQuery] UserRole role)
    {
        var success = await _updateUserRoleCommand.ExecuteAsync(id, role);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _deleteUserCommand.ExecuteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}