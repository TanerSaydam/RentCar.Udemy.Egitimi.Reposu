using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Categories;
using RentCarServer.Domain.Extras;
using RentCarServer.Domain.ProtectionPackages;
using RentCarServer.Domain.ProtectionPackages.ValueObjects;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Vehicles;
using RentCarServer.Domain.Vehicles.ValueObjects;
using TS.Result;

namespace RentCarServer.WebAPI.Modules;

public static class SeedDataModule
{
    public static void MapSeedData(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("seed-data").RequireAuthorization().WithTags("SeedData");

        //categories
        app.MapGet("categories",
            async (ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
            {
                var categoryNames = await categoryRepository.GetAll().Select(s => s.Name.Value).ToListAsync(cancellationToken);

                List<Category> newCategories = new()
                {
                    new(new Name("Binek"), true),
                    new(new Name("Station Wagon"), true),
                    new(new Name("Minibüs"), true),
                    new(new Name("SUV"), true),
                    new(new Name("Üstü Açık"), true),
                    new(new Name("MPV"), true)
                };

                var list = newCategories.Where(p => !categoryNames.Contains(p.Name.Value)).ToList();
                categoryRepository.AddRange(list);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Results.Ok(Result<string>.Succeed("Kategori seed data başarıyla tamamlandı"));
            })
            .Produces<Result<string>>();

        //protection-packages
        app.MapGet("protection-packages",
            async (IProtectionPackageRepository repository, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
            {
                var existingNames = await repository.GetAll().Select(p => p.Name.Value).ToListAsync(cancellationToken);

                var packages = new List<ProtectionPackage>
                {
                    new(
                        new("Mini Güvence Paketi"),
                        new(150),
                        new(false),
                        new(1),
                        new List<ProtectionCoverage>
                        {
                            new("Hasar Sorumluluk Güvencesi (CDW)"),
                            new("Hırsızlık Güvencesi (TP)")
                        },
                        true),

                    new(
                        new("Standart Güvence Paketi"),
                        new(250),
                        new(true),
                        new(2),
                        new List<ProtectionCoverage>
                        {
                            new("Önceki Paketin Tüm Özellikleri Dahil"),
                            new("Mini Pakete Ek: Lastik, Cam, Far Güvencesi (TWH)"),
                            new("Mini Hasar Güvencesi (MI)")
                        },
                        true),

                    new(
                        new("Full Güvence Paketi"),
                        new(350),
                        new(false),
                        new(3),
                        new List<ProtectionCoverage>
                        {
                            new("Önceki Paketin Tüm Özellikleri Dahil"),
                            new("Standart Pakete Ek: Ek Sürücü"),
                            new("Genç Sürücü")
                        },
                        true),
                };

                var newPackages = packages.Where(p => !existingNames.Contains(p.Name.Value)).ToList();
                repository.AddRange(newPackages);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Results.Ok(Result<string>.Succeed("Koruma paketi seed data başarıyla tamamlandı"));
            })
            .Produces<Result<string>>();

        //extras
        app.MapGet("extras",
            async (IExtraRepository repository, IUnitOfWork unitOfWork, CancellationToken cancellationToken) =>
            {
                var existingNames = await repository.GetAll().Select(e => e.Name.Value).ToListAsync(cancellationToken);

                List<Extra> extras = new()
                {
                    // Önerilen Güvenceler
                    new(new("Mini Hasar Güvencesi"), new(114), new("Ehliyeti ibrazı, kiralama ön şartlarının kabulü (findeksiz, depozito hariç) ve araç teslimi sırasında ek sürücünün de ofiste bizzat bulunması gereklidir."), true),
                    new(new("Kış Lastiği"), new(246), new("Kış Lastiği (stoklarla sınırlıdır)"), true),

                    // Ek Sürücü Paketi
                    new(new("Genç Sürücü Paketi"), new(530), new("Yaş grubunuzun üst yaş grubundaki aracı kiralayabilmenizi sağlamaktadır."), true),
                    new(new("Banka Kartı ile Kiralama"), new(3193), new("Kiralama koşulları geçerli Banka Kartı ile kiralamalar için istenilen müşteriler bu ürünü satın alarak araç kiralamaya devam edebilir."), true),
                    new(new("Depozitosuz Kiralama"), new(1064), new("Depozito ödemek istemeyen müşteriler bu ürünle araç kiralayabilir. Kontrat sonunda çalınmaması kaydıyla depozito gibi talep edilmez."), true),

                    // Koltuk Adaptörü
                    new(new("Koltuk Adaptörü"), new(290), new("4 yaşından sonra (15–36 kg.) arası çocuklar için arka koltuk yükseltici koltuklar kullanılmalıdır."), true),
                    new(new("Çocuk Koltuğu"), new(290), new("4 yaşına kadar (9–18 kg.) çocuk güvenlik koltuğu araçta monte edilir."), true),
                    new(new("Bebek Koltuğu"), new(290), new("0 yaş (0 kilo) bebekler için, arka koltuğa monte edilen ana kucağı modelidir."), true),
                };

                var newExtras = extras.Where(e => !existingNames.Contains(e.Name.Value)).ToList();
                repository.AddRange(newExtras);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Results.Ok(Result<string>.Succeed("Extra seed data başarıyla tamamlandı"));
            })
            .Produces<Result<string>>();

        //vehicles
        app.MapGet("vehicles",
            async (
                IVehicleRepository vehicleRepository,
                ICategoryRepository categoryRepository,
                IUnitOfWork unitOfWork,
                CancellationToken cancellationToken) =>
            {
                var branchId = Guid.Parse("0197de0b-7613-7846-af49-b5a2cc121576");
                var existingPlates = await vehicleRepository.GetAll().Select(v => v.Plate.Value).ToListAsync(cancellationToken);
                var categories = await categoryRepository.GetAll().ToListAsync(cancellationToken);

                var imageUrls = new[]
                {
                    "citroen-c3.jpg",
                    "fiat-egea-sedan.jpg",
                    "fiat-fiorino.jpg",
                    "renault-clio.jpg",
                    "hyundai-bayon.jpg",
                    "renault-megane-sedan.jpg",
                    "suzuki-vitara.jpg"
                };

                var brands = new[] { "Toyota", "Volkswagen", "Renault", "Ford", "Hundai", "Peugeot", "Opel", "Honda", "BMW" };
                var models = new[] { "C3", "Egea", "Fiorino", "Clio", "Bayon", "Megane", "Vitara" };
                var allFeatures = new[]
                {
                    "Airbag", "ABS", "ESP", "Alarm Sistemi",
                    "GPS Navigasyon", "Park Sensörü", "Geri Görüş Kamerası", "Cruise Control",
                    "Klima", "Isıtmalı Koltuk", "Sunroof", "Bluetooth",
                    "Dokunmatik Ekran", "USB Bağlantısı", "Premium Ses Sistemi", "Apple CarPlay"
                };
                var vehicles = new List<Vehicle>();
                var random = new Random();
                int imageIndex = 0;

                foreach (var category in categories)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var featureCount = new Random().Next(4, 9); // 4-8 arası özellik
                        var selectedFeatures = allFeatures.OrderBy(_ => new Random().Next()).Take(featureCount).ToList();
                        var features = selectedFeatures.Select(f => new Feature(f)).ToList();
                        var brand = brands[random.Next(brands.Length)];
                        var model = models[random.Next(models.Length)];
                        var plate = $"34{brand[..2].ToUpper()}{category.Name.Value[..1].ToUpper()}{i + 1}";

                        if (existingPlates.Contains(plate))
                            continue;

                        var vehicle = new Vehicle(
                            new Brand(brand),
                            new Model($"{model} {category.Name.Value[..2].ToUpper()}"),
                            new ModelYear(2022 + i % 2),
                            new Color("Gri"),
                            new Plate(plate),
                            new IdentityId(category.Id),
                            new IdentityId(branchId),
                            new VinNumber($"VIN{category.Name.Value[..1].ToUpper()}{i}{Guid.NewGuid():N}".Substring(0, 16)),
                            new EngineNumber($"ENG{i}{Guid.NewGuid():N}".Substring(0, 12)),
                            new Description($"{brand} {model} açıklaması"),
                            new ImageUrl(imageUrls[imageIndex++ % imageUrls.Length]),
                            new FuelType(i % 2 == 0 ? "Benzin" : "Dizel"),
                            new Transmission(i % 2 == 0 ? "Otomatik" : "Manuel"),
                            new EngineVolume(1.4m + i * 0.1m),
                            new EnginePower(100 + i * 10),
                            new TractionType("Önden Çekiş"),
                            new FuelConsumption(5.5m + i * 0.3m),
                            new SeatCount(5),
                            new Kilometer(10000 + i * 3000),
                            new DailyPrice(1800 + i * 50),
                            new WeeklyDiscountRate(10),
                            new MonthlyDiscountRate(15),
                            new InsuranceType("Kasko & Sigorta"),
                            new LastMaintenanceDate(DateOnly.FromDateTime(DateTime.Now.AddMonths(-3))),
                            new LastMaintenanceKm(15000 + i * 1000),
                            new NextMaintenanceKm(20000 + i * 1000),
                            new InspectionDate(DateOnly.FromDateTime(DateTime.Now.AddMonths(9))),
                            new InsuranceEndDate(DateOnly.FromDateTime(DateTime.Now.AddYears(1))),
                            new CascoEndDate(DateOnly.FromDateTime(DateTime.Now.AddYears(1))),
                            new TireStatus("İyi"),
                            new GeneralStatus("Çok İyi"),
                            features,
                            true
                        );

                        vehicles.Add(vehicle);
                    }
                }

                vehicleRepository.AddRange(vehicles);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Results.Ok(Result<string>.Succeed("Tüm araçlar başarıyla eklendi"));
            })
            .Produces<Result<string>>();

    }
}