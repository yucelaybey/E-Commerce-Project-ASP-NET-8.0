using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ming_Favorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoritesCards",
                columns: table => new
                {
                    FavoritesCardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritesCards", x => x.FavoritesCardID);
                });

            migrationBuilder.CreateTable(
                name: "FavoritesCardItems",
                columns: table => new
                {
                    FavoritesCardItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FavoritesCardID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritesCardItems", x => x.FavoritesCardItemID);
                    table.ForeignKey(
                        name: "FK_FavoritesCardItems_FavoritesCards_FavoritesCardID",
                        column: x => x.FavoritesCardID,
                        principalTable: "FavoritesCards",
                        principalColumn: "FavoritesCardID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritesCardItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoritesCardItems_FavoritesCardID",
                table: "FavoritesCardItems",
                column: "FavoritesCardID");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritesCardItems_ProductID",
                table: "FavoritesCardItems",
                column: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritesCardItems");

            migrationBuilder.DropTable(
                name: "FavoritesCards");
        }
    }
}
