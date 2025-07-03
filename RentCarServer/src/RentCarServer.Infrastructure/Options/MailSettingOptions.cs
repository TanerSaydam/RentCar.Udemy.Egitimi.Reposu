namespace RentCarServer.Infrastructure.Options;
public sealed class MailSettingOptions
{
    public string Email { get; set; } = default!;
    public string Smtp { get; set; } = default!;
    public int Port { get; set; }
    public bool SSL { get; set; }
    public string UserId { get; set; } = default!;
    public string Password { get; set; } = default!;
}
