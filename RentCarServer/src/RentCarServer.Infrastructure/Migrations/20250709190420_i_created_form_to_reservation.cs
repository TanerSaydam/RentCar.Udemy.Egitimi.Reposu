using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i_created_form_to_reservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryForm_Kilometer_Value",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PickUpForm_Kilometer_Value",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeliveryForm_Damages",
                columns: table => new
                {
                    FormReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryForm_Damages", x => new { x.FormReservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_DeliveryForm_Damages_Reservations_FormReservationId",
                        column: x => x.FormReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryForm_ImageUrls",
                columns: table => new
                {
                    FormReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryForm_ImageUrls", x => new { x.FormReservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_DeliveryForm_ImageUrls_Reservations_FormReservationId",
                        column: x => x.FormReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryForm_Note",
                columns: table => new
                {
                    FormReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryForm_Note", x => x.FormReservationId);
                    table.ForeignKey(
                        name: "FK_DeliveryForm_Note_Reservations_FormReservationId",
                        column: x => x.FormReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryForm_Supplies",
                columns: table => new
                {
                    FormReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryForm_Supplies", x => new { x.FormReservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_DeliveryForm_Supplies_Reservations_FormReservationId",
                        column: x => x.FormReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickUpForm_Damages",
                columns: table => new
                {
                    FormReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickUpForm_Damages", x => new { x.FormReservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_PickUpForm_Damages_Reservations_FormReservationId",
                        column: x => x.FormReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickUpForm_ImageUrls",
                columns: table => new
                {
                    FormReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickUpForm_ImageUrls", x => new { x.FormReservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_PickUpForm_ImageUrls_Reservations_FormReservationId",
                        column: x => x.FormReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickUpForm_Note",
                columns: table => new
                {
                    FormReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickUpForm_Note", x => x.FormReservationId);
                    table.ForeignKey(
                        name: "FK_PickUpForm_Note_Reservations_FormReservationId",
                        column: x => x.FormReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickUpForm_Supplies",
                columns: table => new
                {
                    FormReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickUpForm_Supplies", x => new { x.FormReservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_PickUpForm_Supplies_Reservations_FormReservationId",
                        column: x => x.FormReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryForm_Damages");

            migrationBuilder.DropTable(
                name: "DeliveryForm_ImageUrls");

            migrationBuilder.DropTable(
                name: "DeliveryForm_Note");

            migrationBuilder.DropTable(
                name: "DeliveryForm_Supplies");

            migrationBuilder.DropTable(
                name: "PickUpForm_Damages");

            migrationBuilder.DropTable(
                name: "PickUpForm_ImageUrls");

            migrationBuilder.DropTable(
                name: "PickUpForm_Note");

            migrationBuilder.DropTable(
                name: "PickUpForm_Supplies");

            migrationBuilder.DropColumn(
                name: "DeliveryForm_Kilometer_Value",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PickUpForm_Kilometer_Value",
                table: "Reservations");
        }
    }
}
