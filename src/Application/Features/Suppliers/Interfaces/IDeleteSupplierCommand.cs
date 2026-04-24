namespace core_first.Application.Features.Suppliers.Interfaces;

public interface IDeleteSupplierCommand
{
    Task<bool> ExecuteAsync(int id);
}