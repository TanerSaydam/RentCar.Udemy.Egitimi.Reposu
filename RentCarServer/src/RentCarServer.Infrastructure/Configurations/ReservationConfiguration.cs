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
        builder.OwnsMany(p => p.Histories);
        builder.OwnsOne(p => p.PickUpForm, modelBuilder =>
        {
            modelBuilder.OwnsOne(i => i.Kilometer);
            modelBuilder.OwnsMany(i => i.ImageUrls, x =>
            {
                x.ToTable("PickUpForm_ImageUrls");
            });

            modelBuilder.OwnsMany(i => i.Supplies, x =>
            {
                x.ToTable("PickUpForm_Supplies");
            });

            modelBuilder.OwnsMany(i => i.Damages, x =>
            {
                x.ToTable("PickUpForm_Damages");
            });

            modelBuilder.OwnsOne(i => i.Note, x =>
            {
                x.ToTable("PickUpForm_Note");
            });
        });
        builder.OwnsOne(p => p.DeliveryForm, modelBuilder =>
        {
            modelBuilder.OwnsOne(i => i.Kilometer);
            modelBuilder.OwnsMany(i => i.ImageUrls, x =>
            {
                x.ToTable("DeliveryForm_ImageUrls");
            });

            modelBuilder.OwnsMany(i => i.Supplies, x =>
            {
                x.ToTable("DeliveryForm_Supplies");
            });

            modelBuilder.OwnsMany(i => i.Damages, x =>
            {
                x.ToTable("DeliveryForm_Damages");
            });

            modelBuilder.OwnsOne(i => i.Note, x =>
            {
                x.ToTable("DeliveryForm_Note");
            });
        });
    }
}
