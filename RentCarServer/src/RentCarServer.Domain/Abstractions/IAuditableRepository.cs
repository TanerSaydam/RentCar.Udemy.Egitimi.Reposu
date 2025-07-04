using GenericRepository;
using RentCarServer.Domain.Users;

namespace RentCarServer.Domain.Abstractions;
public interface IAuditableRepository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    IQueryable<EntityWithAuditDto<TEntity>> GetAllWithAudit();
}

public sealed class EntityWithAuditDto<TEntity>
    where TEntity : Entity
{
    public TEntity Entity { get; set; } = default!;
    public User CreatedUser { get; set; } = default!;
    public User? UpdatedUser { get; set; }
}