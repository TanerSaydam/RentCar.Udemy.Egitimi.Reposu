using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Domain.Customers;

public interface ICustomerRepository : IAuditableRepository<Customer>
{
}