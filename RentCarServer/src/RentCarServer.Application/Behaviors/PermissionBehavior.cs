using System.Reflection;
using RentCarServer.Application.Services;
using TS.MediatR;

namespace RentCarServer.Application.Behaviors;
public sealed class PermissionBehavior<TRequest, TResponse>(
    IUserContext userContext) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
    {
        var attr = request.GetType().GetCustomAttribute<PermissionAttribute>(inherit: true);

        if (attr is null) return await next();

        var userId = userContext.GetUserId();
        //var user = await userRepository.FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);
        //if (user is null)
        //{
        //    throw new ArgumentException("User bulunamadı");
        //}

        //// Eğer permission string'i varsa kontrol et
        //if (!string.IsNullOrEmpty(attr.Permission))
        //{
        //    var hasPermission = user.Permissions.Any(p => p.Name == attr.Permission);
        //    if (!hasPermission)
        //    {
        //        throw new AuthorizationException($"'{attr.Permission}' yetkisine sahip değilsiniz.");
        //    }
        //}

        //// Eğer permission string'i yoksa sadece admin kontrolü yap
        //else if (!user.IsAdmin.Value)
        //{
        //    throw new AuthorizationException("Bu işlem için admin yetkisi gereklidir.");
        //}

        return await next();
    }
}

public sealed class PermissionAttribute : Attribute
{
    public string? Permission { get; }

    public PermissionAttribute()
    {
    }

    public PermissionAttribute(string permission)
    {
        Permission = permission;
    }
}

public sealed class AuthorizationException : Exception
{
    public AuthorizationException() : base("Yetkiniz bulunmamaktadır.")
    {
    }

    public AuthorizationException(string message) : base(message)
    {
    }
}