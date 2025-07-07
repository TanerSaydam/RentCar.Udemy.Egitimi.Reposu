using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i_added_total_field_and_convert_to_extra_to_table_in_the_reservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "ExtraPrice_Value",
                table: "Reservations",
                newName: "Total_Value");

            migrationBuilder.CreateTable(
                name: "ReservationExtra",
                columns: table => new
                {
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExtraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationExtra", x => new { x.ReservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_ReservationExtra_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationExtra");

            migrationBuilder.RenameColumn(
                name: "Total_Value",
                table: "Reservations",
                newName: "ExtraPrice_Value");

            migrationBuilder.AddColumn<Guid>(
                name: "ExtraId",
                table: "Reservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
