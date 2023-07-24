using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidencePojisteni.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsuredUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insured_AspNetUsers_UzivatelId",
                table: "Insured");

            migrationBuilder.RenameColumn(
                name: "UzivatelId",
                table: "Insured",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Insured_UzivatelId",
                table: "Insured",
                newName: "IX_Insured_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insured_AspNetUsers_UserId",
                table: "Insured",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insured_AspNetUsers_UserId",
                table: "Insured");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Insured",
                newName: "UzivatelId");

            migrationBuilder.RenameIndex(
                name: "IX_Insured_UserId",
                table: "Insured",
                newName: "IX_Insured_UzivatelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insured_AspNetUsers_UzivatelId",
                table: "Insured",
                column: "UzivatelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
