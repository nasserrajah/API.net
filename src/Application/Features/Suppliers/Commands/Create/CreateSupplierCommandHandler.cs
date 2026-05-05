using MediatR;
using core_first.Application.Features.Suppliers.DTOs;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Entities;

namespace core_first.Application.Features.Suppliers.Commands.Create;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierDto>
{
    private readonly ISupplierRepository _supplierRepository;
    public CreateSupplierCommandHandler(ISupplierRepository supplierRepository) => _supplierRepository = supplierRepository;

    public async Task<SupplierDto> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Supplier;
        var supplier = new Supplier
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _supplierRepository.AddAsync(supplier);
        return new SupplierDto
        {
            Id = created.Id,
            Name = created.Name,
            Phone = created.Phone,
            Email = created.Email,
            Address = created.Address
        };
    }
}