using RentCarServer.Domain.Reservations.Forms.ValueObjects;
using RentCarServer.Domain.Reservations.ValueObjects;
using RentCarServer.Domain.Vehicles.ValueObjects;

namespace RentCarServer.Domain.Reservations.Forms;

public sealed record Form
{
    private readonly List<Supplies> _supplies = new();
    private readonly List<ImageUrl> _imageUrls = new();
    private readonly List<Damage> _damages = new();
    private Form() { }
    public Form(
        Kilometer kilometer,
        List<Supplies> supplies,
        List<ImageUrl> imageUrls,
        List<Damage> damages,
        Note note
        )
    {
        SetKilometer(kilometer);
        SetSupplies(supplies);
        SetImageUrls(imageUrls);
        SetDamages(damages);
        SetNote(note);
    }

    public Kilometer Kilometer { get; private set; } = default!;
    public IReadOnlyCollection<Supplies> Supplies => _supplies;
    public IReadOnlyCollection<ImageUrl> ImageUrls => _imageUrls;
    public IReadOnlyCollection<Damage> Damages => _damages;
    public Note Note { get; private set; } = default!;

    #region Behaviors
    public void SetKilometer(Kilometer kilometer)
    {
        Kilometer = kilometer;
    }

    public void SetSupplies(List<Supplies> vehicleSupplies)
    {
        _supplies.Clear();
        _supplies.AddRange(vehicleSupplies);
    }

    public void SetImageUrls(List<ImageUrl> imageUrls)
    {
        _imageUrls.Clear();
        _imageUrls.AddRange(imageUrls);
    }

    public void SetDamages(List<Damage> vehicleDamages)
    {
        _damages.Clear();
        _damages.AddRange(vehicleDamages);
    }

    public void SetNote(Note note)
    {
        Note = note;
    }
    #endregion
}
