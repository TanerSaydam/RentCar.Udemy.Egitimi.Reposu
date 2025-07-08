namespace RentCarServer.Domain.Reservations.ValueObjects;

public sealed record ReservationHistory(
    string Title,
    string Description,
    DateTimeOffset CreatedAt);