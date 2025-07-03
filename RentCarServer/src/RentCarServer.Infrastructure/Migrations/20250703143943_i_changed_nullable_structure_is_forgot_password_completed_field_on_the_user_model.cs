using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i_changed_nullable_structure_is_forgot_password_completed_field_on_the_user_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsForgotPasswordCompleted_Value",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsForgotPasswordCompleted_Value",
                table: "Users",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
