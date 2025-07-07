using RentCarServer.Domain.Customers;
using TS.MediatR;

namespace RentCarServer.Application.Customers;

public sealed record CustomerGetAllQuery : IRequest<IQueryable<CustomerDto>>;

internal sealed class CustomerGetAllQueryHandler(
    ICustomerRepository repository) : IRequestHandler<CustomerGetAllQuery, IQueryable<CustomerDto>>
{
    public Task<IQueryable<CustomerDto>> Handle(CustomerGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(repository.GetAllWithAudit().MapTo().AsQueryable());
}