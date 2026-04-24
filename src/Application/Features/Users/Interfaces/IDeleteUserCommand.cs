namespace core_first.Application.Features.Users.Interfaces;

public interface IDeleteUserCommand
{
    Task<bool> ExecuteAsync(int id);
}