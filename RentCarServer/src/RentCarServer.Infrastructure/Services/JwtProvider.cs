using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentCarServer.Application.Services;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.LoginTokens.ValueObjects;
using RentCarServer.Domain.Users;
using RentCarServer.Infrastructure.Options;

namespace RentCarServer.Infrastructure.Services;
internal sealed class JwtProvider(
    ILoginTokenRepository loginTokenRepository,
    IUnitOfWork unitOfWork,
    IOptions<JwtOptions> options) : IJwtProvider
{
    public async Task<string> CreateTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("fullName", user.FullName.Value),
            new Claim("email",user.Email.Value)
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        var expires = DateTime.Now.AddDays(1);
        JwtSecurityToken securityToken = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(securityToken);

        Token newToken = new(token);
        ExpiresDate expiresDate = new(expires);
        LoginToken loginToken = new(newToken, user.Id, expiresDate);
        loginTokenRepository.Add(loginToken);

        var loginTokens = await loginTokenRepository
            .Where(p => p.UserId == user.Id && p.IsActive.Value == true)
            .ToListAsync(cancellationToken);
        foreach (var item in loginTokens)
        {
            item.SetIsActive(new(false));
        }
        loginTokenRepository.UpdateRange(loginTokens);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }
}
