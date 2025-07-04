using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Users;

namespace RentCarServer.Application;
internal static class ExtensionMethods
{
    public static IQueryable<EntityWithAuditDto<TEntity>> ApplyAuditDto<TEntity>(this IQueryable<TEntity> entities, IQueryable<User> users)
        where TEntity : Entity
    {
        var res = entities
           .Join(users, m => m.CreatedBy, m => m.Id, (b, user) =>
                   new { entity = b, createdUser = user })
           .GroupJoin(users, m => m.entity.UpdatedBy, m => m.Id, (b, user) =>
                   new { b.entity, b.createdUser, updatedUser = user })
           .SelectMany(s => s.updatedUser.DefaultIfEmpty(),
               (x, updatedUser) => new EntityWithAuditDto<TEntity>
               {
                   Entity = x.entity,
                   CreatedUser = x.createdUser,
                   UpdatedUser = updatedUser
               });

        return res;
    }
}

public sealed class EntityWithAuditDto<TEntity>
    where TEntity : Entity
{
    public TEntity Entity { get; set; } = default!;
    public User CreatedUser { get; set; } = default!;
    public User? UpdatedUser { get; set; }
}
