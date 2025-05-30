using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "PaymentCards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCards_AppUserId",
                table: "PaymentCards",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCards_AppUsers_AppUserId",
                table: "PaymentCards",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentCards_AppUsers_AppUserId",
                table: "PaymentCards");

            migrationBuilder.DropIndex(
                name: "IX_PaymentCards_AppUserId",
                table: "PaymentCards");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "PaymentCards");
        }
    }
}
