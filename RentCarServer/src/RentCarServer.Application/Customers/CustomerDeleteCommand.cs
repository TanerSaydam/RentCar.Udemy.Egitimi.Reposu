using GenericRepository;
using RentCarServer.Domain.Customers;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Customers;

public sealed record CustomerDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class CustomerDeleteCommandHandler(
    ICustomerRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CustomerDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await repository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (customer is null)
            return Result<string>.Failure("Müþteri bulunamadý");

        customer.Delete();
        repository.Update(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Müþteri baþarýyla silindi";
    }
}