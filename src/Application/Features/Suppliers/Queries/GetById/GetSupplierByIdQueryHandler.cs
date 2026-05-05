using MediatR;
using core_first.Application.Features.Suppliers.DTOs;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Suppliers.Queries.GetById;

public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, SupplierDto?>
{
    private readonly ISupplierRepository _supplierRepository;
    public GetSupplierByIdQueryHandler(ISupplierRepository supplierRepository) => _supplierRepository = supplierRepository;

    public async Task<SupplierDto?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.Id);
        if (supplier == null) return null;
        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Phone = supplier.Phone,
            Email = supplier.Email,
            Address = supplier.Address
        };
    }
}