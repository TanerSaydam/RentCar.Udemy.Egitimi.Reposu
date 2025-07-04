using GenericRepository;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Auth;
public sealed record LoginWithTFACommand(
    string EmailOrUserName,
    string TFACode,
    string TFAConfirmCode) : IRequest<Result<LoginCommandResponse>>;

internal sealed class LoginWithTFACommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginWithTFACommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginWithTFACommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
            p.Email.Value == request.EmailOrUserName
            || p.UserName.Value == request.EmailOrUserName);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı adı ya da şifre yanlış");
        }

        if (
            user.TFAIsCompleted is null
            || user.TFAExpiresDate is null
            || user.TFACode is null
            || user.TFAConfirmCode is null
            || user.TFAIsCompleted.Value
            || user.TFAExpiresDate.Value < DateTimeOffset.Now
            || (user.TFAConfirmCode.Value != request.TFAConfirmCode
            || user.TFACode.Value != request.TFACode))
        {
            return Result<LoginCommandResponse>.Failure("TFA kodu geçersiz");
        }

        user.SetTFACompleted();
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);
        var res = new LoginCommandResponse() { Token = token };

        return res;
    }
}
