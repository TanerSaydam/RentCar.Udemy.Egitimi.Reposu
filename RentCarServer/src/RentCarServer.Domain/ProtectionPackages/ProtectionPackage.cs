using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.ProtectionPackages.ValueObjects;
using RentCarServer.Domain.Shared;

namespace RentCarServer.Domain.ProtectionPackages;

public sealed class ProtectionPackage : Entity
{
    private readonly List<ProtectionCoverage> _coverages = new();
    private ProtectionPackage() { }

    public ProtectionPackage(
        Name name,
        Price price,
        IsRecommended isRecommended,
        OrderNumber orderNumber,
        IEnumerable<ProtectionCoverage> coverages,
        bool isActive)
    {
        SetName(name);
        SetPrice(price);
        SetIsRecommended(isRecommended);
        SetCoverages(coverages);
        SetStatus(isActive);
    }

    public Name Name { get; private set; } = default!;
    public Price Price { get; private set; } = default!;
    public IsRecommended IsRecommended { get; private set; } = default!;
    public OrderNumber OrderNumber { get; private set; } = default!;
    public IReadOnlyCollection<ProtectionCoverage> Coverages => _coverages;

    public void SetName(Name name) => Name = name;
    public void SetPrice(Price price) => Price = price;
    public void SetIsRecommended(IsRecommended isRecommended) => IsRecommended = isRecommended;
    public void SetOrderNumber(OrderNumber orderNumber) => OrderNumber = orderNumber;
    public void SetCoverages(IEnumerable<ProtectionCoverage> coverages)
    {
        _coverages.Clear();
        _coverages.AddRange(coverages);
    }
}