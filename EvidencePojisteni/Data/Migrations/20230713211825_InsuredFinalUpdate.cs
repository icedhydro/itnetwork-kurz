using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidencePojisteni.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsuredFinalUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insured_AspNetUsers_UserId",
                table: "Insured");

            migrationBuilder.DropIndex(
                name: "IX_Insured_UserId",
                table: "Insured");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Insured");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Insured",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Insured_UserId",
                table: "Insured",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insured_AspNetUsers_UserId",
                table: "Insured",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
