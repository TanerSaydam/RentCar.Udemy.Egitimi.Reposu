using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;

namespace RentCarServer.Infrastructure.Services;
internal sealed class JwtProvider : IJwtProvider
{
    public string CreateToken(User user)
    {
        return "token";
    }
}
