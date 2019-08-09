using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EnterpriseContact.Migrations
{
    public partial class add_MdmDataHistory_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MdmDataHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    HistoryVersion = table.Column<string>(maxLength: 64, nullable: false),
                    SyncTime = table.Column<DateTimeOffset>(nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MdmDataHistories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MdmDataHistories");
        }
    }
}
