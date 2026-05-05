using MediatR;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Customers.Commands.Update;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var existing = await _customerRepository.GetByIdAsync(request.Id);
        if (existing == null) return false;

        var dto = request.Customer;
        existing.Name = dto.Name;
        existing.Phone = dto.Phone;
        existing.Email = dto.Email;
        existing.Address = dto.Address;
        await _customerRepository.UpdateAsync(existing);
        return true;
    }
}