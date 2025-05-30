using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_PaymentTester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderPaymentCardId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderPaymentOtherId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderPaymentCards",
                columns: table => new
                {
                    OrderPaymentCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderCardName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderCardCVV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderCardDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPaymentCards", x => x.OrderPaymentCardId);
                });

            migrationBuilder.CreateTable(
                name: "OrderPaymentOthers",
                columns: table => new
                {
                    OrderPaymentOtherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPaymentOthers", x => x.OrderPaymentOtherId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderPaymentCardId",
                table: "Orders",
                column: "OrderPaymentCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderPaymentOtherId",
                table: "Orders",
                column: "OrderPaymentOtherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderPaymentCards_OrderPaymentCardId",
                table: "Orders",
                column: "OrderPaymentCardId",
                principalTable: "OrderPaymentCards",
                principalColumn: "OrderPaymentCardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderPaymentOthers_OrderPaymentOtherId",
                table: "Orders",
                column: "OrderPaymentOtherId",
                principalTable: "OrderPaymentOthers",
                principalColumn: "OrderPaymentOtherId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderPaymentCards_OrderPaymentCardId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderPaymentOthers_OrderPaymentOtherId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderPaymentCards");

            migrationBuilder.DropTable(
                name: "OrderPaymentOthers");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderPaymentCardId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderPaymentOtherId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderPaymentCardId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderPaymentOtherId",
                table: "Orders");
        }
    }
}
