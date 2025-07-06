using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Domain.Users;
public interface IUserRepository : IAuditableRepository<User>
{
}
