using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentTaskApp.Migrations
{
    /// <inheritdoc />
    public partial class AddVacationIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Vacations_EmployeeId_DateRange",
                table: "Vacations",
                columns: new[] { "EmployeeId", "DateSince", "DateUntil" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vacations_EmployeeId_DateRange",
                table: "Vacations");
        }
    }
}
