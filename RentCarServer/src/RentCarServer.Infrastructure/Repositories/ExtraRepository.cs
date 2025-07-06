using RentCarServer.Domain.Extras;
using RentCarServer.Infrastructure.Abstractions;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;

internal sealed class ExtraRepository : AuditableRepository<Extra, ApplicationDbContext>, IExtraRepository
{
    public ExtraRepository(ApplicationDbContext context) : base(context)
    {
    }
}