using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeEmailUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEntities",
                table: "EmployeeEntities");

            migrationBuilder.RenameTable(
                name: "EmployeeEntities",
                newName: "Employees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Email",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "EmployeeEntities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEntities",
                table: "EmployeeEntities",
                column: "Id");
        }
    }
}
