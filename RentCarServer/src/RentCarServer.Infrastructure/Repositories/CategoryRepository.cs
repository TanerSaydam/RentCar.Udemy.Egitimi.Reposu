using RentCarServer.Domain.Categories;
using RentCarServer.Infrastructure.Abstractions;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;

internal sealed class CategoryRepository : AuditableRepository<Category, ApplicationDbContext>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}