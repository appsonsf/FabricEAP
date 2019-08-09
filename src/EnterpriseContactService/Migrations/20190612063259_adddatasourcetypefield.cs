using Microsoft.EntityFrameworkCore.Migrations;

namespace EnterpriseContact.Migrations
{
    public partial class adddatasourcetypefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataSourceType",
                table: "Positions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataSourceType",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataSourceType",
                table: "EmployeePositions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataSourceType",
                table: "Departments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSourceType",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "DataSourceType",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DataSourceType",
                table: "EmployeePositions");

            migrationBuilder.DropColumn(
                name: "DataSourceType",
                table: "Departments");
        }
    }
}
