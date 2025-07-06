using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i_added_contact_property_to_branch_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_PhoneNumber2",
                table: "Branches",
                newName: "Contact_PhoneNumber2");

            migrationBuilder.RenameColumn(
                name: "Address_PhoneNumber1",
                table: "Branches",
                newName: "Contact_PhoneNumber1");

            migrationBuilder.RenameColumn(
                name: "Address_Email",
                table: "Branches",
                newName: "Contact_Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Contact_PhoneNumber2",
                table: "Branches",
                newName: "Address_PhoneNumber2");

            migrationBuilder.RenameColumn(
                name: "Contact_PhoneNumber1",
                table: "Branches",
                newName: "Address_PhoneNumber1");

            migrationBuilder.RenameColumn(
                name: "Contact_Email",
                table: "Branches",
                newName: "Address_Email");
        }
    }
}
