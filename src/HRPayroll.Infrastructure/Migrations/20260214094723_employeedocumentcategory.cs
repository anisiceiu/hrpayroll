using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRPayroll.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class My_employeedocumentcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "EmployeeDocuments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "EmployeeDocuments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "EmployeeDocuments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "EmployeeDocuments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameBN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    HasExpiryDate = table.Column<bool>(type: "bit", nullable: false),
                    MaxFileSizeMB = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_CategoryId",
                table: "EmployeeDocuments",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDocuments_DocumentCategories_CategoryId",
                table: "EmployeeDocuments",
                column: "CategoryId",
                principalTable: "DocumentCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDocuments_DocumentCategories_CategoryId",
                table: "EmployeeDocuments");

            migrationBuilder.DropTable(
                name: "DocumentCategories");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDocuments_CategoryId",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "EmployeeDocuments");

            migrationBuilder.AlterColumn<decimal>(
                name: "FileSize",
                table: "EmployeeDocuments",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
