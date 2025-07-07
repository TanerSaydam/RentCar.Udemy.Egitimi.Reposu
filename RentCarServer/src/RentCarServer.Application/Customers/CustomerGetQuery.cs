using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.Customers;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Customers;

public sealed record CustomerGetQuery(Guid Id) : IRequest<Result<CustomerDto>>;

internal sealed class CustomerGetQueryHandler(
    ICustomerRepository repository) : IRequestHandler<CustomerGetQuery, Result<CustomerDto>>
{
    public async Task<Result<CustomerDto>> Handle(CustomerGetQuery request, CancellationToken cancellationToken)
    {
        var res = await repository
            .GetAllWithAudit()
            .MapTo()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (res is null)
            return Result<CustomerDto>.Failure("Müþteri bulunamadý");

        return res;
    }
}