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

[Permission("vehicle:update")]
public sealed record VehicleUpdateCommand(
    Guid Id,
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
    DateTimeOffset LastMaintenanceDate,
    int LastMaintenanceKm,
    int NextMaintenanceKm,
    DateTimeOffset InspectionDate,
    DateTimeOffset InsuranceEndDate,
    DateTimeOffset CascoEndDate,
    string TireStatus,
    string GeneralStatus,
    List<string> Features,
    IFormFile? File,
    bool IsActive
) : IRequest<Result<string>>;

public sealed class VehicleUpdateCommandValidator : AbstractValidator<VehicleUpdateCommand>
{
    public VehicleUpdateCommandValidator()
    {
        RuleFor(p => p.Brand).NotEmpty();
        RuleFor(p => p.Model).NotEmpty();
        RuleFor(p => p.ModelYear).GreaterThan(1900);
        RuleFor(p => p.Plate).NotEmpty();
    }
}

internal sealed class VehicleUpdateCommandHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<VehicleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(VehicleUpdateCommand request, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await vehicleRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (vehicle is null)
            return Result<string>.Failure("Araç bulunamadý");

        if (!string.Equals(vehicle.Plate.Value, request.Plate, StringComparison.OrdinalIgnoreCase))
        {
            bool plateExists = await vehicleRepository.AnyAsync(
                p => p.Plate.Value == request.Plate && p.Id != request.Id, cancellationToken);
            if (plateExists)
                return Result<string>.Failure("Bu plaka ile kayýtlý baþka bir araç var.");
        }

        string imageUrl = vehicle.ImageUrl.Value;
        if (request.File is not null && request.File.Length > 0)
        {
            imageUrl = FileService.FileSaveToServer(request.File, "wwwroot/images/");
        }

        vehicle.SetBrand(new Brand(request.Brand));
        vehicle.SetModel(new Model(request.Model));
        vehicle.SetModelYear(new ModelYear(request.ModelYear));
        vehicle.SetColor(new Color(request.Color));
        vehicle.SetPlate(new Plate(request.Plate));
        vehicle.SetCategoryId(new IdentityId(request.CategoryId));
        vehicle.SetBranchId(new IdentityId(request.BranchId));
        vehicle.SetVinNumber(new VinNumber(request.VinNumber));
        vehicle.SetEngineNumber(new EngineNumber(request.EngineNumber));
        vehicle.SetDescription(new Description(request.Description));
        vehicle.SetImageUrl(new ImageUrl(imageUrl));
        vehicle.SetFuelType(new FuelType(request.FuelType));
        vehicle.SetTransmission(new Transmission(request.Transmission));
        vehicle.SetEngineVolume(new EngineVolume(request.EngineVolume));
        vehicle.SetEnginePower(new EnginePower(request.EnginePower));
        vehicle.SetTractionType(new TractionType(request.TractionType));
        vehicle.SetFuelConsumption(new FuelConsumption(request.FuelConsumption));
        vehicle.SetSeatCount(new SeatCount(request.SeatCount));
        vehicle.SetKilometer(new Kilometer(request.Kilometer));
        vehicle.SetDailyPrice(new DailyPrice(request.DailyPrice));
        vehicle.SetWeeklyDiscountRate(new WeeklyDiscountRate(request.WeeklyDiscountRate));
        vehicle.SetMonthlyDiscountRate(new MonthlyDiscountRate(request.MonthlyDiscountRate));
        vehicle.SetInsuranceType(new InsuranceType(request.InsuranceType));
        vehicle.SetLastMaintenanceDate(new LastMaintenanceDate(request.LastMaintenanceDate));
        vehicle.SetLastMaintenanceKm(new LastMaintenanceKm(request.LastMaintenanceKm));
        vehicle.SetNextMaintenanceKm(new NextMaintenanceKm(request.NextMaintenanceKm));
        vehicle.SetInspectionDate(new InspectionDate(request.InspectionDate));
        vehicle.SetInsuranceEndDate(new InsuranceEndDate(request.InsuranceEndDate));
        vehicle.SetCascoEndDate(new CascoEndDate(request.CascoEndDate));
        vehicle.SetTireStatus(new TireStatus(request.TireStatus));
        vehicle.SetGeneralStatus(new GeneralStatus(request.GeneralStatus));
        vehicle.SetFeatures(request.Features.Select(f => new Feature(f)));
        vehicle.SetStatus(request.IsActive);

        vehicleRepository.Update(vehicle);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Araç baþarýyla güncellendi";
    }
}