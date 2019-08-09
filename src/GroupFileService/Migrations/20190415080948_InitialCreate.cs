using Microsoft.EntityFrameworkCore.Migrations;

namespace GroupFile.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileItems_FileStores_StoreId",
                table: "FileItems");

            migrationBuilder.DropTable(
                name: "FileStores");

            migrationBuilder.DropIndex(
                name: "IX_FileItems_StoreId",
                table: "FileItems");

            migrationBuilder.AlterColumn<string>(
                name: "StoreId",
                table: "FileItems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StoreId",
                table: "FileItems",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FileStores",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileItems_StoreId",
                table: "FileItems",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileItems_FileStores_StoreId",
                table: "FileItems",
                column: "StoreId",
                principalTable: "FileStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
