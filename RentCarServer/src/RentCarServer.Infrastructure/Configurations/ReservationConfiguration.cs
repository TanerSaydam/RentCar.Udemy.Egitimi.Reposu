using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Reservations;

namespace RentCarServer.Infrastructure.Configurations;
internal sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");
        builder.HasKey(x => x.Id);

        builder.OwnsOne(p => p.ReservationNumber);
        builder.OwnsOne(p => p.PickUpDate);
        builder.OwnsOne(p => p.PickUpTime);
        builder.OwnsOne(p => p.DeliveryDate);
        builder.OwnsOne(p => p.DeliveryTime);
        builder.OwnsOne(p => p.TotalDay);
        builder.OwnsOne(p => p.VehicleDailyPrice);
        builder.OwnsOne(p => p.ProtectionPackagePrice);
        builder.OwnsMany(p => p.ReservationExtras);
        builder.OwnsOne(p => p.Note);
        builder.OwnsOne(p => p.PaymentInformation);
        builder.OwnsOne(p => p.Status);
        builder.OwnsOne(p => p.Total);
        builder.OwnsOne(p => p.PickUpDatetime);
        builder.OwnsOne(p => p.DeliveryDatetime);
    }
}
