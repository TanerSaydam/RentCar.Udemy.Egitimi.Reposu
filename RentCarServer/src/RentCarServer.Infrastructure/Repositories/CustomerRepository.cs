using RentCarServer.Domain.Customers;
using RentCarServer.Infrastructure.Abstractions;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;

internal sealed class CustomerRepository : AuditableRepository<Customer, ApplicationDbContext>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }
}