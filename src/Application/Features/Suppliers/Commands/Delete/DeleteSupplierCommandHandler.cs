using MediatR;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Suppliers.Commands.Delete;

public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, bool>
{
    private readonly ISupplierRepository _supplierRepository;
    public DeleteSupplierCommandHandler(ISupplierRepository supplierRepository) => _supplierRepository = supplierRepository;

    public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.Id);
        if (supplier == null) return false;
        await _supplierRepository.DeleteAsync(supplier);
        return true;
    }
}