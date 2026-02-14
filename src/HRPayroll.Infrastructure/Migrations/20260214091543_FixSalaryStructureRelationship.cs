using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRPayroll.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSalaryStructureRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryStructures_Employees_EmployeeId1",
                table: "SalaryStructures");

            migrationBuilder.DropIndex(
                name: "IX_SalaryStructures_EmployeeId",
                table: "SalaryStructures");

            migrationBuilder.DropIndex(
                name: "IX_SalaryStructures_EmployeeId1",
                table: "SalaryStructures");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "SalaryStructures");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructures_EmployeeId",
                table: "SalaryStructures",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalaryStructures_EmployeeId",
                table: "SalaryStructures");

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId1",
                table: "SalaryStructures",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructures_EmployeeId",
                table: "SalaryStructures",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryStructures_EmployeeId1",
                table: "SalaryStructures",
                column: "EmployeeId1",
                unique: true,
                filter: "[EmployeeId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryStructures_Employees_EmployeeId1",
                table: "SalaryStructures",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
