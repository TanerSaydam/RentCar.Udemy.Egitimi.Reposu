using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Domain.Categories;

public interface ICategoryRepository : IAuditableRepository<Category>
{
}