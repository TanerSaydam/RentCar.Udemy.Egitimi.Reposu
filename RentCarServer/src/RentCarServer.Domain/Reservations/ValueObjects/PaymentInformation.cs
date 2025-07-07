namespace RentCarServer.Domain.Reservations.ValueObjects;

public sealed record PaymentInformation(
    string CartNumber,
    string Owner);
