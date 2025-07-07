using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Vehicles.ValueObjects;

namespace RentCarServer.Domain.Vehicles;

public sealed class Vehicle : Entity
{
    private readonly List<Feature> _features = new();
    private Vehicle() { }

    public Vehicle(
        Brand brand,
        Model model,
        ModelYear modelYear,
        Color color,
        Plate plate,
        IdentityId categoryId,
        IdentityId branchId,
        VinNumber vinNumber,
        EngineNumber engineNumber,
        Description description,
        ImageUrl imageUrl,
        FuelType fuelType,
        Transmission transmission,
        EngineVolume engineVolume,
        EnginePower enginePower,
        TractionType tractionType,
        FuelConsumption fuelConsumption,
        SeatCount seatCount,
        Kilometer kilometer,
        DailyPrice dailyPrice,
        WeeklyDiscountRate weeklyDiscountRate,
        MonthlyDiscountRate monthlyDiscountRate,
        InsuranceType insuranceType,
        LastMaintenanceDate lastMaintenanceDate,
        LastMaintenanceKm lastMaintenanceKm,
        NextMaintenanceKm nextMaintenanceKm,
        InspectionDate inspectionDate,
        InsuranceEndDate insuranceEndDate,
        CascoEndDate cascoEndDate,
        TireStatus tireStatus,
        GeneralStatus generalStatus,
        IEnumerable<Feature> features,
        bool isActive)
    {
        SetBrand(brand);
        SetModel(model);
        SetModelYear(modelYear);
        SetColor(color);
        SetPlate(plate);
        SetCategoryId(categoryId);
        SetBranchId(branchId);
        SetVinNumber(vinNumber);
        SetEngineNumber(engineNumber);
        SetDescription(description);
        SetImageUrl(imageUrl);
        SetFuelType(fuelType);
        SetTransmission(transmission);
        SetEngineVolume(engineVolume);
        SetEnginePower(enginePower);
        SetTractionType(tractionType);
        SetFuelConsumption(fuelConsumption);
        SetSeatCount(seatCount);
        SetKilometer(kilometer);
        SetDailyPrice(dailyPrice);
        SetWeeklyDiscountRate(weeklyDiscountRate);
        SetMonthlyDiscountRate(monthlyDiscountRate);
        SetInsuranceType(insuranceType);
        SetLastMaintenanceDate(lastMaintenanceDate);
        SetLastMaintenanceKm(lastMaintenanceKm);
        SetNextMaintenanceKm(nextMaintenanceKm);
        SetInspectionDate(inspectionDate);
        SetInsuranceEndDate(insuranceEndDate);
        SetCascoEndDate(cascoEndDate);
        SetTireStatus(tireStatus);
        SetGeneralStatus(generalStatus);
        SetFeatures(features);
        SetStatus(isActive);
    }

    public Brand Brand { get; private set; } = default!;
    public Model Model { get; private set; } = default!;
    public ModelYear ModelYear { get; private set; } = default!;
    public Color Color { get; private set; } = default!;
    public Plate Plate { get; private set; } = default!;
    public IdentityId CategoryId { get; private set; } = default!;
    public IdentityId BranchId { get; private set; } = default!;
    public VinNumber VinNumber { get; private set; } = default!;
    public EngineNumber EngineNumber { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public ImageUrl ImageUrl { get; private set; } = default!;
    public FuelType FuelType { get; private set; } = default!;
    public Transmission Transmission { get; private set; } = default!;
    public EngineVolume EngineVolume { get; private set; } = default!;
    public EnginePower EnginePower { get; private set; } = default!;
    public TractionType TractionType { get; private set; } = default!;
    public FuelConsumption FuelConsumption { get; private set; } = default!;
    public SeatCount SeatCount { get; private set; } = default!;
    public Kilometer Kilometer { get; private set; } = default!;
    public DailyPrice DailyPrice { get; private set; } = default!;
    public WeeklyDiscountRate WeeklyDiscountRate { get; private set; } = default!;
    public MonthlyDiscountRate MonthlyDiscountRate { get; private set; } = default!;
    public InsuranceType InsuranceType { get; private set; } = default!;
    public LastMaintenanceDate LastMaintenanceDate { get; private set; } = default!;
    public LastMaintenanceKm LastMaintenanceKm { get; private set; } = default!;
    public NextMaintenanceKm NextMaintenanceKm { get; private set; } = default!;
    public InspectionDate InspectionDate { get; private set; } = default!;
    public InsuranceEndDate InsuranceEndDate { get; private set; } = default!;
    public CascoEndDate CascoEndDate { get; private set; } = default!;
    public TireStatus TireStatus { get; private set; } = default!;
    public GeneralStatus GeneralStatus { get; private set; } = default!;
    public IReadOnlyCollection<Feature> Features => _features;

    #region Behaviors
    public void SetBrand(Brand brand) => Brand = brand;
    public void SetModel(Model model) => Model = model;
    public void SetModelYear(ModelYear modelYear) => ModelYear = modelYear;
    public void SetColor(Color color) => Color = color;
    public void SetPlate(Plate plate) => Plate = plate;
    public void SetCategoryId(IdentityId categoryId) => CategoryId = categoryId;
    public void SetBranchId(IdentityId branchId) => BranchId = branchId;
    public void SetVinNumber(VinNumber vinNumber) => VinNumber = vinNumber;
    public void SetEngineNumber(EngineNumber engineNumber) => EngineNumber = engineNumber;
    public void SetDescription(Description description) => Description = description;
    public void SetImageUrl(ImageUrl imageUrl) => ImageUrl = imageUrl;
    public void SetFuelType(FuelType fuelType) => FuelType = fuelType;
    public void SetTransmission(Transmission transmission) => Transmission = transmission;
    public void SetEngineVolume(EngineVolume engineVolume) => EngineVolume = engineVolume;
    public void SetEnginePower(EnginePower enginePower) => EnginePower = enginePower;
    public void SetTractionType(TractionType tractionType) => TractionType = tractionType;
    public void SetFuelConsumption(FuelConsumption fuelConsumption) => FuelConsumption = fuelConsumption;
    public void SetSeatCount(SeatCount seatCount) => SeatCount = seatCount;
    public void SetKilometer(Kilometer kilometer) => Kilometer = kilometer;
    public void SetDailyPrice(DailyPrice dailyPrice) => DailyPrice = dailyPrice;
    public void SetWeeklyDiscountRate(WeeklyDiscountRate weeklyDiscountRate) => WeeklyDiscountRate = weeklyDiscountRate;
    public void SetMonthlyDiscountRate(MonthlyDiscountRate monthlyDiscountRate) => MonthlyDiscountRate = monthlyDiscountRate;
    public void SetInsuranceType(InsuranceType insuranceType) => InsuranceType = insuranceType;
    public void SetLastMaintenanceDate(LastMaintenanceDate lastMaintenanceDate) => LastMaintenanceDate = lastMaintenanceDate;
    public void SetLastMaintenanceKm(LastMaintenanceKm lastMaintenanceKm) => LastMaintenanceKm = lastMaintenanceKm;
    public void SetNextMaintenanceKm(NextMaintenanceKm nextMaintenanceKm) => NextMaintenanceKm = nextMaintenanceKm;
    public void SetInspectionDate(InspectionDate inspectionDate) => InspectionDate = inspectionDate;
    public void SetInsuranceEndDate(InsuranceEndDate insuranceEndDate) => InsuranceEndDate = insuranceEndDate;
    public void SetCascoEndDate(CascoEndDate cascoEndDate) => CascoEndDate = cascoEndDate;
    public void SetTireStatus(TireStatus tireStatus) => TireStatus = tireStatus;
    public void SetGeneralStatus(GeneralStatus generalStatus) => GeneralStatus = generalStatus;
    public void SetFeatures(IEnumerable<Feature> features)
    {
        _features.Clear();
        _features.AddRange(features);
    }
    #endregion
}