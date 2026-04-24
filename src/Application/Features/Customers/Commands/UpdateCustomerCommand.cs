using core_first.Application.Features.Customers.Interfaces;
using core_first.Application.Features.Customers.DTOs;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Customers.Commands;

public class UpdateCustomerCommand : IUpdateCustomerCommand
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommand(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<bool> ExecuteAsync(int id, CustomerDto customerDto)
    {
        var existing = await _customerRepository.GetByIdAsync(id);
        if (existing == null) return false;

        existing.Name = customerDto.Name;
        existing.Phone = customerDto.Phone;
        existing.Email = customerDto.Email;
        existing.Address = customerDto.Address;
        await _customerRepository.UpdateAsync(existing);
        return true;
    }
}