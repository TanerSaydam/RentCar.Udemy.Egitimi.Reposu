using RentCarServer.Domain.Categories;
using TS.MediatR;

namespace RentCarServer.Application.Categories;

public sealed record CategoryGetAllQuery : IRequest<IQueryable<CategoryDto>>;

internal sealed class CategoryGetAllQueryHandler(
    ICategoryRepository categoryRepository) : IRequestHandler<CategoryGetAllQuery, IQueryable<CategoryDto>>
{
    public Task<IQueryable<CategoryDto>> Handle(CategoryGetAllQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(categoryRepository.GetAllWithAudit().MapToGet().AsQueryable());
}