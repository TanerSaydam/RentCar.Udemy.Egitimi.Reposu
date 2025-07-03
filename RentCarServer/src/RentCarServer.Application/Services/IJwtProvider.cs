using RentCarServer.Domain.Users;

namespace RentCarServer.Application.Services;
public interface IJwtProvider
{
    string CreateToken(User user);
}
