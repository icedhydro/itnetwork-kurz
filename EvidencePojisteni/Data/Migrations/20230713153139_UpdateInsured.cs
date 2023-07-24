using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidencePojisteni.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInsured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UzivatelId",
                table: "Insured",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insured_UzivatelId",
                table: "Insured",
                column: "UzivatelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insured_AspNetUsers_UzivatelId",
                table: "Insured",
                column: "UzivatelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insured_AspNetUsers_UzivatelId",
                table: "Insured");

            migrationBuilder.DropIndex(
                name: "IX_Insured_UzivatelId",
                table: "Insured");

            migrationBuilder.DropColumn(
                name: "UzivatelId",
                table: "Insured");
        }
    }
}
