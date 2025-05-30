using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_orderttt23432 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderAddressId",
                table: "Orders",
                newName: "OrderPaymentAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderPaymentAddressId",
                table: "Orders",
                column: "OrderPaymentAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderPaymentAddresses_OrderPaymentAddressId",
                table: "Orders",
                column: "OrderPaymentAddressId",
                principalTable: "OrderPaymentAddresses",
                principalColumn: "OrderPaymentAddressId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderPaymentAddresses_OrderPaymentAddressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderPaymentAddressId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "OrderPaymentAddressId",
                table: "Orders",
                newName: "OrderAddressId");
        }
    }
}
