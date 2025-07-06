using FluentValidation;
using GenericRepository;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.Shared;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Categories;
[Permission("category:create")]
public sealed record CategoryCreateCommand(
    string Name,
    bool IsActive) : IRequest<Result<string>>;

public sealed class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
{
    public CategoryCreateCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Ge�erli bir kategori ad� girin");
    }
}

internal sealed class CategoryCreateCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CategoryCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await categoryRepository.AnyAsync(p => p.Name.Value == request.Name, cancellationToken);

        if (nameExists)
        {
            return Result<string>.Failure("Kategori ad� daha �nce tan�mlanm��");
        }

        Name name = new(request.Name);
        Category category = new(name, request.IsActive);
        categoryRepository.Add(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kategori ba�ar�yla kaydedildi";
    }
}