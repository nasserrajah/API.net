using MediatR;
using core_first.Application.Features.Suppliers.DTOs;
using core_first.Application.Interfaces.Repositories;

namespace core_first.Application.Features.Suppliers.Queries.GetAll;

public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDto>>
{
    private readonly ISupplierRepository _supplierRepository;
    public GetAllSuppliersQueryHandler(ISupplierRepository supplierRepository) => _supplierRepository = supplierRepository;

    public async Task<IEnumerable<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _supplierRepository.GetFilteredAsync(request.Name, request.Phone, request.Email);
        return suppliers.Select(s => new SupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            Phone = s.Phone,
            Email = s.Email,
            Address = s.Address
        });
    }
}