using GenericRepository;
using RentCarServer.Domain.Reservations;
using RentCarServer.Domain.Reservations.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Reservations;
public sealed record ReservationDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class ReservationDeleteCommandHandler(
    IReservationRepository reservationRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ReservationDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ReservationDeleteCommand request, CancellationToken cancellationToken)
    {
        Reservation? reservation = await reservationRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (reservation is null)
        {
            return Result<string>.Failure("Rezervasyon bulunamadı");
        }

        if (reservation.Status != Status.Pending)
        {
            return Result<string>.Failure("Bu rezervasyon değiştirilemez");
        }

        reservation.Delete();
        reservationRepository.Update(reservation);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rezervasyon başarıyla silindi";
    }
}