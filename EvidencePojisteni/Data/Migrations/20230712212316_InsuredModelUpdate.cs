using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidencePojisteni.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsuredModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurance_Insured_InsuredId",
                table: "Insurance");

            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "Insured");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Insured");

            migrationBuilder.AlterColumn<int>(
                name: "InsuredId",
                table: "Insurance",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Insurance_Insured_InsuredId",
                table: "Insurance",
                column: "InsuredId",
                principalTable: "Insured",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurance_Insured_InsuredId",
                table: "Insurance");

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

            migrationBuilder.AlterColumn<int>(
                name: "InsuredId",
                table: "Insurance",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurance_Insured_InsuredId",
                table: "Insurance",
                column: "InsuredId",
                principalTable: "Insured",
                principalColumn: "Id");
        }
    }
}
