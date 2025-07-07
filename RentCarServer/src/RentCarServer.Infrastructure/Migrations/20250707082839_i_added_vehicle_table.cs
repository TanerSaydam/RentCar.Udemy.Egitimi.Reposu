using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i_added_vehicle_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Brand_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Model_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    ModelYear_Value = table.Column<int>(type: "int", nullable: false),
                    Color_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Plate_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    CategoryId_Value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId_Value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VinNumber_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    EngineNumber_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Description_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    ImageUrl_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    FuelType_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Transmission_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    EngineVolume_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnginePower_Value = table.Column<int>(type: "int", nullable: false),
                    TractionType_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    FuelConsumption_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SeatCount_Value = table.Column<int>(type: "int", nullable: false),
                    Kilometer_Value = table.Column<int>(type: "int", nullable: false),
                    DailyPrice_Value = table.Column<decimal>(type: "money", nullable: false),
                    WeeklyDiscountRate_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyDiscountRate_Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsuranceType_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    LastMaintenanceDate_Value = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastMaintenanceKm_Value = table.Column<int>(type: "int", nullable: false),
                    NextMaintenanceKm_Value = table.Column<int>(type: "int", nullable: false),
                    InspectionDate_Value = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InsuranceEndDate_Value = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CascoEndDate_Value = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TireStatus_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    GeneralStatus_Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => new { x.VehicleId, x.Id });
                    table.ForeignKey(
                        name: "FK_Feature_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feature");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
