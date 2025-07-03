using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;
public sealed record CheckForgotPasswordCodeCommand(
    Guid ForgotPasswordCode) : IRequest<Result<bool>>;

internal sealed class CheckForgotPasswordCodeCommandHandler(
    IUserRepository userRepository) : IRequestHandler<CheckForgotPasswordCodeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CheckForgotPasswordCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
            p.ForgotPasswordCode != null
            && p.ForgotPasswordCode.Value == request.ForgotPasswordCode
            && p.IsForgotPasswordCompleted.Value == false
            , cancellationToken);

        if (user is null)
        {
            return Result<bool>.Failure("Şifre sıfırlama değeriniz geçersiz");
        }

        var fpDate = user.ForgotPasswordDate!.Value.AddDays(1);
        var now = DateTimeOffset.Now;
        if (fpDate < now)
        {
            return Result<bool>.Failure("Şifre sıfırlama değeriniz geçersiz");
        }

        return true;
    }
}
