using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvidencePojisteni.Data.Migrations
{
    /// <inheritdoc />
    public partial class RepairUpdateInsurances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ammount",
                table: "Insurance",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Ammount",
                table: "Event",
                newName: "Amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Insurance",
                newName: "Ammount");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Event",
                newName: "Ammount");
        }
    }
}
