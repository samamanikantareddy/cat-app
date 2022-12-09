using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cat_app.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavouriteCats",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    url = table.Column<string>(type: "TEXT", nullable: false),
                    width = table.Column<int>(type: "INTEGER", nullable: false),
                    height = table.Column<int>(type: "INTEGER", nullable: false),
                    FanId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteCats", x => x.id);
                    table.ForeignKey(
                        name: "FK_FavouriteCats_AspNetUsers_FanId",
                        column: x => x.FanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteCats_FanId",
                table: "FavouriteCats",
                column: "FanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteCats");
        }
    }
}
