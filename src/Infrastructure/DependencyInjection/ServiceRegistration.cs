using Microsoft.Extensions.DependencyInjection;
using core_first.Application.Interfaces.Repositories;
using core_first.Application.Interfaces.Services;
using core_first.Infrastructure.Persistence;
using core_first.Infrastructure.Repositories;
using core_first.Infrastructure.Services;

using core_first.Application.Features.Auth.Interfaces;
using core_first.Application.Features.Auth.Queries;
using core_first.Application.Features.Auth.Commands;

using core_first.Application.Features.Customers.Interfaces;
using core_first.Application.Features.Customers.Queries;
using core_first.Application.Features.Customers.Commands;

using core_first.Application.Features.Suppliers.Interfaces;
using core_first.Application.Features.Suppliers.Queries;
using core_first.Application.Features.Suppliers.Commands;

using core_first.Application.Features.Users.Interfaces;
using core_first.Application.Features.Users.Queries;
using core_first.Application.Features.Users.Commands;

namespace core_first.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();

        // Infrastructure Services
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }

    public static IServiceCollection AddApplicationFeatures(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<ILoginQuery, LoginQuery>();
        services.AddScoped<IRegisterCommand, RegisterCommand>();

        // Customers
        services.AddScoped<IGetAllCustomersQuery, GetAllCustomersQuery>();
        services.AddScoped<IGetCustomerByIdQuery, GetCustomerByIdQuery>();
        services.AddScoped<ICreateCustomerCommand, CreateCustomerCommand>();
        services.AddScoped<IUpdateCustomerCommand, UpdateCustomerCommand>();
        services.AddScoped<IDeleteCustomerCommand, DeleteCustomerCommand>();

        // Suppliers
        services.AddScoped<IGetAllSuppliersQuery, GetAllSuppliersQuery>();
        services.AddScoped<IGetSupplierByIdQuery, GetSupplierByIdQuery>();
        services.AddScoped<ICreateSupplierCommand, CreateSupplierCommand>();
        services.AddScoped<IUpdateSupplierCommand, UpdateSupplierCommand>();
        services.AddScoped<IDeleteSupplierCommand, DeleteSupplierCommand>();

        // Users
        services.AddScoped<IGetAllUsersQuery, GetAllUsersQuery>();
        services.AddScoped<IGetUserByIdQuery, GetUserByIdQuery>();
        services.AddScoped<IUpdateUserRoleCommand, UpdateUserRoleCommand>();
        services.AddScoped<IDeleteUserCommand, DeleteUserCommand>();

        return services;
    }
}