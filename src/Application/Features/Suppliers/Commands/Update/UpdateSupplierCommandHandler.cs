using MediatR;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Suppliers.Commands.Update;

public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, bool>
{
    private readonly ISupplierRepository _supplierRepository;
    public UpdateSupplierCommandHandler(ISupplierRepository supplierRepository) => _supplierRepository = supplierRepository;

    public async Task<bool> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var existing = await _supplierRepository.GetByIdAsync(request.Id);
        if (existing == null) return false;

        var dto = request.Supplier;
        existing.Name = dto.Name;
        existing.Phone = dto.Phone;
        existing.Email = dto.Email;
        existing.Address = dto.Address;
        await _supplierRepository.UpdateAsync(existing);
        return true;
    }
}