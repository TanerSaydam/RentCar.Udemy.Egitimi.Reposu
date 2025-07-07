using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Domain.Reservations;
public interface IReservationRepository : IAuditableRepository<Reservation>;