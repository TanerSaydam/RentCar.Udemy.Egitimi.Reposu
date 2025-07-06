using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Infrastructure.Abstractions;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;

internal sealed class ProtectionPackageRepository : AuditableRepository<ProtectionPackage, ApplicationDbContext>, IProtectionPackageRepository
{
    public ProtectionPackageRepository(ApplicationDbContext context) : base(context)
    {
    }
}