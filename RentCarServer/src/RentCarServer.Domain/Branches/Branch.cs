using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Shared;

namespace RentCarServer.Domain.Branches;
public sealed class Branch : Entity
{
    private Branch() { }
    public Branch(
        Name name,
        Address address,
        Contact contact,
        bool isActive)
    {
        SetName(name);
        SetAddress(address);
        SetContact(contact);
        SetStatus(isActive);
    }
    public Name Name { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public Contact Contact { get; private set; } = default!;

    #region Behaviors
    public void SetName(Name name)
    {
        Name = name;
    }

    public void SetAddress(Address address)
    {
        Address = address;
    }

    public void SetContact(Contact contact)
    {
        Contact = contact;
    }
    #endregion
}