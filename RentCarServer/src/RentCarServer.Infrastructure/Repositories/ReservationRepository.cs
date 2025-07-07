using RentCarServer.Domain.Reservations;
using RentCarServer.Infrastructure.Abstractions;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;
internal sealed class ReservationRepository : AuditableRepository<Reservation, ApplicationDbContext>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context)
    {
    }
}