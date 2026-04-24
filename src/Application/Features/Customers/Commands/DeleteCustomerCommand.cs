using core_first.Application.Features.Customers.Interfaces;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Customers.Commands;

public class DeleteCustomerCommand : IDeleteCustomerCommand
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommand(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return false;
        await _customerRepository.DeleteAsync(customer);
        return true;
    }
}