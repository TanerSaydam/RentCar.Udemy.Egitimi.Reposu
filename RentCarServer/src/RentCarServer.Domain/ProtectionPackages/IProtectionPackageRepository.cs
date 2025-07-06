using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Domain.ProtectionPackages;

public interface IProtectionPackageRepository : IAuditableRepository<ProtectionPackage>
{
}