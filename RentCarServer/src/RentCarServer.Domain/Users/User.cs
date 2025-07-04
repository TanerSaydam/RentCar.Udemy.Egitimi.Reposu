using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Users.ValueObjects;

namespace RentCarServer.Domain.Users;
public sealed class User : Entity
{
    public User(
        FirstName firstName,
        LastName lastName,
        Email email,
        UserName userName,
        Password password)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetEmail(email);
        SetUserName(userName);
        SetPassword(password);
        SetFullName();
        SetIsForgotPasswordCompleted(new(true));
    }

    private User() { }
    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public FullName FullName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public UserName UserName { get; private set; } = default!;
    public Password Password { get; private set; } = default!;
    public ForgotPasswordCode? ForgotPasswordCode { get; private set; }
    public ForgotPasswordDate? ForgotPasswordDate { get; private set; }
    public IsForgotPasswordCompleted IsForgotPasswordCompleted { get; private set; } = default!;

    #region Behaviors
    public bool VerifyPasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Password.PasswordHash);
    }

    public void CreateForgotPasswordId()
    {
        ForgotPasswordCode = new(Guid.CreateVersion7());
        ForgotPasswordDate = new(DateTimeOffset.Now);
        IsForgotPasswordCompleted = new(false);
    }

    public void SetFirstName(FirstName firstName)
    {
        FirstName = firstName;
    }

    public void SetLastName(LastName lastName)
    {
        LastName = lastName;
    }

    public void SetEmail(Email email)
    {
        Email = email;
    }

    public void SetUserName(UserName userName)
    {
        UserName = userName;
    }

    public void SetFullName()
    {
        FullName = new(FirstName.Value + " " + LastName.Value + " (" + Email.Value + ")");
    }

    public void SetPassword(Password password)
    {
        Password = password;
    }

    public void SetIsForgotPasswordCompleted(IsForgotPasswordCompleted isForgotPasswordCompleted)
    {
        IsForgotPasswordCompleted = isForgotPasswordCompleted;
    }
    #endregion
}
