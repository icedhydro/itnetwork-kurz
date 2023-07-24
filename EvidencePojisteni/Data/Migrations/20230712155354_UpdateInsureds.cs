using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidencePojisteni.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInsureds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "Insured",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Insured",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "Insured");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Insured");
        }
    }
}
