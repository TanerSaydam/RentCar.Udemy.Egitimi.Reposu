using GenericRepository;
using RentCarServer.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Categories;

public sealed record CategoryDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class CategoryDeleteCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CategoryDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (category is null)
        {
            return Result<string>.Failure("Kategori bulunamadý");
        }

        category.Delete();
        categoryRepository.Update(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kategori baþarýyla silindi";
    }
}