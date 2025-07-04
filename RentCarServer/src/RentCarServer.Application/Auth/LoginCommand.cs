using FluentValidation;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;
public sealed record LoginCommand(
    string EmailOrUserName,
    string Password) : IRequest<Result<string>>;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(p => p.EmailOrUserName).NotEmpty().WithMessage("Geçerli bir mail ya da kullanıcı adı girin");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Geçerli bir şifre girin");
    }
}

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
            p.Email.Value == request.EmailOrUserName
            || p.UserName.Value == request.EmailOrUserName);

        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı adı ya da şifre yanlış");
        }

        var checkPassword = user.VerifyPasswordHash(request.Password);

        if (!checkPassword)
        {
            return Result<string>.Failure("Kullanıcı adı ya da şifre yanlış");
        }

        var token = jwtProvider.CreateTokenAsync(user);

        return token;
    }
}