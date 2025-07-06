namespace RentCarServer.Domain.Shared;

public sealed record Address(
    string City,
    string District,
    string FullAddress
    );