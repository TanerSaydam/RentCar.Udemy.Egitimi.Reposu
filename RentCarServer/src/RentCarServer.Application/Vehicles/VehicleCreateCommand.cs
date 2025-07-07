using FluentValidation;
using GenericFileService.Files;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Vehicles;
using RentCarServer.Domain.Vehicles.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Vehicles;

[Permission("vehicle:create")]
public sealed record VehicleCreateCommand(
    string Brand,
    string Model,
    int ModelYear,
    string Color,
    string Plate,
    Guid CategoryId,
    Guid BranchId,
    string VinNumber,
    string EngineNumber,
    string Description,
    string FuelType,
    string Transmission,
    decimal EngineVolume,
    int EnginePower,
    string TractionType,
    decimal FuelConsumption,
    int SeatCount,
    int Kilometer,
    decimal DailyPrice,
    decimal WeeklyDiscountRate,
    decimal MonthlyDiscountRate,
    string InsuranceType,
    DateOnly LastMaintenanceDate,
    int LastMaintenanceKm,
    int NextMaintenanceKm,
    DateOnly InspectionDate,
    DateOnly InsuranceEndDate,
    DateOnly? CascoEndDate,
    string TireStatus,
    string GeneralStatus,
    List<string> Features,
    IFormFile File,
    bool IsActive
) : IRequest<Result<string>>;

public sealed class VehicleCreateCommandValidator : AbstractValidator<VehicleCreateCommand>
{
    public VehicleCreateCommandValidator()
    {
        RuleFor(p => p.Brand)
            .NotEmpty()
            .WithMessage("Marka alaný boþ býrakýlamaz.");

        RuleFor(p => p.Model)
            .NotEmpty()
            .WithMessage("Model alaný boþ býrakýlamaz.");

        RuleFor(p => p.ModelYear)
            .GreaterThan(1900)
            .WithMessage("Geçerli bir model yýlý seçmelisiniz.");

        RuleFor(p => p.Plate)
            .NotEmpty()
            .WithMessage("Plaka bilgisi girilmelidir.");

        RuleFor(p => p.File)
            .NotEmpty()
            .WithMessage("Araç görseli yüklemelisiniz.");

        RuleFor(p => p.Features)
            .Must(i => i != null && i.Any())
            .WithMessage("En az bir özellik seçmelisiniz.");
    }

}

internal sealed class VehicleCreateCommandHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<VehicleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(VehicleCreateCommand request, CancellationToken cancellationToken)
    {
        if (await vehicleRepository.AnyAsync(p => p.Plate.Value == request.Plate, cancellationToken))
            return Result<string>.Failure("Bu plaka ile kayýtlý baþka bir araç var.");

        string fileName = FileService.FileSaveToServer(request.File, "wwwroot/images/");

        Brand brand = new(request.Brand);
        Model model = new(request.Model);
        ModelYear modelYear = new(request.ModelYear);
        Color color = new(request.Color);
        Plate plate = new(request.Plate);
        IdentityId categoryId = new(request.CategoryId);
        IdentityId branchId = new(request.BranchId);
        VinNumber vinNumber = new(request.VinNumber);
        EngineNumber engineNumber = new(request.EngineNumber);
        Description description = new(request.Description);
        ImageUrl imageUrl = new(fileName);
        FuelType fuelType = new(request.FuelType);
        Transmission transmission = new(request.Transmission);
        EngineVolume engineVolume = new(request.EngineVolume);
        EnginePower enginePower = new(request.EnginePower);
        TractionType tractionType = new(request.TractionType);
        FuelConsumption fuelConsumption = new(request.FuelConsumption);
        SeatCount seatCount = new(request.SeatCount);
        Kilometer kilometer = new(request.Kilometer);
        DailyPrice dailyPrice = new(request.DailyPrice);
        WeeklyDiscountRate weeklyDiscountRate = new(request.WeeklyDiscountRate);
        MonthlyDiscountRate monthlyDiscountRate = new(request.MonthlyDiscountRate);
        InsuranceType insuranceType = new(request.InsuranceType);
        LastMaintenanceDate lastMaintenanceDate = new(request.LastMaintenanceDate);
        LastMaintenanceKm lastMaintenanceKm = new(request.LastMaintenanceKm);
        NextMaintenanceKm nextMaintenanceKm = new(request.NextMaintenanceKm);
        InspectionDate inspectionDate = new(request.InspectionDate);
        InsuranceEndDate insuranceEndDate = new(request.InsuranceEndDate);
        CascoEndDate? cascoEndDate = request.CascoEndDate is not null ? new(request.CascoEndDate.Value) : null;
        TireStatus tireStatus = new(request.TireStatus);
        GeneralStatus generalStatus = new(request.GeneralStatus);
        IEnumerable<Feature> features = request.Features.Select(f => new Feature(f));

        Vehicle vehicle = new Vehicle(
            brand,
            model,
            modelYear,
            color,
            plate,
            categoryId,
            branchId,
            vinNumber,
            engineNumber,
            description,
            imageUrl,
            fuelType,
            transmission,
            engineVolume,
            enginePower,
            tractionType,
            fuelConsumption,
            seatCount,
            kilometer,
            dailyPrice,
            weeklyDiscountRate,
            monthlyDiscountRate,
            insuranceType,
            lastMaintenanceDate,
            lastMaintenanceKm,
            nextMaintenanceKm,
            inspectionDate,
            insuranceEndDate,
            cascoEndDate,
            tireStatus,
            generalStatus,
            features,
            request.IsActive
        );

        vehicleRepository.Add(vehicle);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Araç baþarýyla kaydedildi";
    }
}