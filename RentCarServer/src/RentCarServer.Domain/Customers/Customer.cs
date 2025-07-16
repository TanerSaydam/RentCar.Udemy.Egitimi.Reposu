using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Users.ValueObjects;

namespace RentCarServer.Domain.Customers;

public sealed class Customer : Entity
{
    private Customer() { }

    public Customer(
        FirstName firstName,
        LastName lastName,
        IdentityNumber identityNumber,
        DateOfBirth dateOfBirth,
        PhoneNumber phoneNumber,
        Email email,
        DrivingLicenseIssuanceDate drivingLicenseIssuanceDate,
        FullAddress fullAddress,
        Password password,
        bool isActive)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetFullName();
        SetIdentityNumber(identityNumber);
        SetDateOfBirth(dateOfBirth);
        SetPhoneNumber(phoneNumber);
        SetEmail(email);
        SetDrivingLicenseIssuanceDate(drivingLicenseIssuanceDate);
        SetFullAddress(fullAddress);
        SetStatus(isActive);
        SetPassword(password);
    }

    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public FullName FullName { get; private set; } = default!;
    public IdentityNumber IdentityNumber { get; private set; } = default!;
    public DateOfBirth DateOfBirth { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public DrivingLicenseIssuanceDate DrivingLicenseIssuanceDate { get; private set; } = default!;
    public FullAddress FullAddress { get; private set; } = default!;
    public Password Password { get; private set; } = default!;

    #region Behaviors
    public void SetFirstName(FirstName firstName) => FirstName = firstName;
    public void SetLastName(LastName lastName) => LastName = lastName;
    public void SetFullName() => FullName = new(string.Join(" ", FirstName.Value, LastName.Value));
    public void SetIdentityNumber(IdentityNumber identityNumber) => IdentityNumber = identityNumber;
    public void SetDateOfBirth(DateOfBirth dateOfBirth) => DateOfBirth = dateOfBirth;
    public void SetPhoneNumber(PhoneNumber phoneNumber) => PhoneNumber = phoneNumber;
    public void SetEmail(Email email) => Email = email;
    public void SetDrivingLicenseIssuanceDate(DrivingLicenseIssuanceDate date) => DrivingLicenseIssuanceDate = date;
    public void SetFullAddress(FullAddress fullAddress) => FullAddress = fullAddress;
    public void SetPassword(Password password) => Password = password;
    public bool VerifyPasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Password.PasswordHash);
    }
    #endregion
}