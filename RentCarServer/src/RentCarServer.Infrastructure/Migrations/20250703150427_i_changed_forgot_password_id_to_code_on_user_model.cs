using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i_changed_forgot_password_id_to_code_on_user_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForgotPasswordId_Value",
                table: "Users",
                newName: "ForgotPasswordCode_Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForgotPasswordCode_Value",
                table: "Users",
                newName: "ForgotPasswordId_Value");
        }
    }
}
