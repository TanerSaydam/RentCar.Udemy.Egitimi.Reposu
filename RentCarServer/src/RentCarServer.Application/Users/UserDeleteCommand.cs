using GenericRepository;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Users;
[Permission("user:delete")]
public sealed record UserDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class UserDeleteCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UserDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı");
        }

        if (user.UserName.Value == "admin")
        {
            return Result<string>.Failure("Admin kullanıcısı silinemez");
        }

        user.Delete();
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı başarıyla silindi";
    }
}