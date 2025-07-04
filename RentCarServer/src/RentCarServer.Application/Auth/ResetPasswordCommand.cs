using FluentValidation;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.Users;
using RentCarServer.Domain.Users.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;
public sealed record ResetPasswordCommand(
    Guid ForgotPasswordCode,
    string NewPassword,
    bool LogoutAllDevices) : IRequest<Result<string>>;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(p => p.NewPassword).NotEmpty().WithMessage("Geçerli bir yeni şifre girin");
    }
}

internal sealed class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    ILoginTokenRepository loginTokenRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
        p.ForgotPasswordCode != null
        && p.ForgotPasswordCode.Value == request.ForgotPasswordCode
        && p.IsForgotPasswordCompleted.Value == false
        , cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("Şifre sıfırlama değeriniz geçersiz");
        }

        var fpDate = user.ForgotPasswordDate!.Value.AddDays(1);
        var now = DateTimeOffset.Now;
        if (fpDate < now)
        {
            return Result<string>.Failure("Şifre sıfırlama değeriniz geçersiz");
        }

        Password password = new(request.NewPassword);
        user.SetPassword(password);
        userRepository.Update(user);

        if (request.LogoutAllDevices)
        {
            var loginTokens = await loginTokenRepository
                .Where(p => p.UserId == user.Id & p.IsActive.Value == true)
                .ToListAsync(cancellationToken);

            foreach (var item in loginTokens)
            {
                item.SetIsActive(new(false));
            }
            loginTokenRepository.UpdateRange(loginTokens);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Şife başarıyla sıfırlandı. Yeni şifrenizle giriş yapabilirsiniz";
    }
}
