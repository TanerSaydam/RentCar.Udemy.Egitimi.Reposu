namespace RentCarServer.Domain.Reservations;

public sealed record ReservationExtra(
    Guid ExtraId,
    decimal Price);