using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_shoppingitemcheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCardItems_ShoppingCards_ShoppingCardID",
                table: "ShoppingCardItems");

            migrationBuilder.RenameColumn(
                name: "ShoppingCardID",
                table: "ShoppingCardItems",
                newName: "ShoppingCardId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCardItems_ShoppingCardID",
                table: "ShoppingCardItems",
                newName: "IX_ShoppingCardItems_ShoppingCardId");

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingCardId",
                table: "ShoppingCardItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCardItems_ShoppingCards_ShoppingCardId",
                table: "ShoppingCardItems",
                column: "ShoppingCardId",
                principalTable: "ShoppingCards",
                principalColumn: "ShoppingCardID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCardItems_ShoppingCards_ShoppingCardId",
                table: "ShoppingCardItems");

            migrationBuilder.RenameColumn(
                name: "ShoppingCardId",
                table: "ShoppingCardItems",
                newName: "ShoppingCardID");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCardItems_ShoppingCardId",
                table: "ShoppingCardItems",
                newName: "IX_ShoppingCardItems_ShoppingCardID");

            migrationBuilder.AlterColumn<int>(
                name: "ShoppingCardID",
                table: "ShoppingCardItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCardItems_ShoppingCards_ShoppingCardID",
                table: "ShoppingCardItems",
                column: "ShoppingCardID",
                principalTable: "ShoppingCards",
                principalColumn: "ShoppingCardID");
        }
    }
}
