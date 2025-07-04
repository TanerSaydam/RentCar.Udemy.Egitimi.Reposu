using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branchs.ValueObjects;

namespace RentCarServer.Domain.Branches;
public sealed class Branch : Entity
{
    private Branch() { }
    public Branch(
        Name name,
        Address address,
        bool isActive)
    {
        SetName(name);
        SetAddress(address);
        SetStatus(isActive);
    }
    public Name Name { get; private set; } = default!;
    public Address Address { get; private set; } = default!;

    #region Behaviors
    public void SetName(Name name)
    {
        Name = name;
    }

    public void SetAddress(Address address)
    {
        Address = address;
    }
    #endregion
}