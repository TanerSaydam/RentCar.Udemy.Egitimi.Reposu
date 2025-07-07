namespace RentCarServer.Domain.Reservations.ValueObjects;

public sealed record Status(string Value)
{
    public static Status Pending => new("Bekliyor");
    public static Status Delivered => new("Teslim Edildi");
    public static Status Completed => new("Tamamlandı");
    public static Status Canceled => new("İptal Edildi");
}