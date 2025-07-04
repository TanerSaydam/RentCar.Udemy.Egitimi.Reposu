using GenericRepository;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;
internal sealed class LoginTokenRepository : Repository<LoginToken, ApplicationDbContext>, ILoginTokenRepository
{
    public LoginTokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
