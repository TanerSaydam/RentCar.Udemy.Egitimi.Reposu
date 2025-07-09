using GenericFileService.Files;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Reservations.Forms;
using RentCarServer.Domain.Reservations.Forms.ValueObjects;
using RentCarServer.Domain.Reservations.ValueObjects;
using RentCarServer.Domain.Vehicles.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Reservations.Forms;
public sealed record FormUpdateCommand(
    Guid ReservationId,
    string Type,
    int Kilometer,
    List<IFormFile> Files,
    List<string>? Supplies,
    List<Damage>? Damages,
    string? Note) : IRequest<Result<string>>;

internal sealed class FormUpdateCommandHandler(
    IReservationRepository reservationRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<FormUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(FormUpdateCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.FirstOrDefaultAsync(i => i.Id == request.ReservationId, cancellationToken);

        if (reservation is null)
        {
            return Result<string>.Failure("Rezervasyon bulunamadı");
        }

        Kilometer kilometer = new(request.Kilometer);
        List<Supplies> supplies = request.Supplies?.Select(s => new Supplies(s)).ToList() ?? [];
        List<ImageUrl> imageUrls = request.Files.Select(s =>
        {
            var fileName = FileService.FileSaveToServer(s, "wwwroot/forms/");
            return new ImageUrl(fileName);
        }).ToList();
        List<Damage> damages = request.Damages ?? [];
        Note note = new(request.Note ?? string.Empty);

        Form form;
        string message;
        if (request.Type == "pickup")
        {
            form = reservation.PickUpForm;
            message = "Araç müşteriye teslim edildi";
            reservation.SetReservationStatus(Status.Delivered);
            ReservationHistory history = new(
                "Araç Teslim Edildi", "Araç müşteriye teslim edildi", DateTimeOffset.Now);
            reservation.SetHistory(history);
        }
        else
        {
            form = reservation.DeliveryForm;
            message = "Araç müşteriden teslim alındı";
            reservation.SetReservationStatus(Status.Completed);
            ReservationHistory history = new(
                "Araç Geri Alındı", "Araç müşteriden geri alındı", DateTimeOffset.Now);
            reservation.SetHistory(history);
        }

        form.SetKilometer(kilometer);
        form.SetSupplies(supplies);
        form.SetImageUrls(imageUrls);
        form.SetDamages(damages);
        form.SetNote(note);

        if (request.Type == "pickup")
        {
            Form deliveryForm = new(
                form.Kilometer,
                form.Supplies.ToList(),
                [],
                form.Damages.ToList(),
                new(string.Empty));
            reservation.SetDeliveryForm(deliveryForm);
        }

        reservationRepository.Update(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return message;
    }
}