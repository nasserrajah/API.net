namespace core_first.Application.Features.Customers.Interfaces;

public interface IDeleteCustomerCommand
{
    Task<bool> ExecuteAsync(int id);
}