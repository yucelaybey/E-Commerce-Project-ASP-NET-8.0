using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_23424234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Towns_CityId",
                table: "Towns",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Quarters_CityId",
                table: "Quarters",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Quarters_TownId",
                table: "Quarters",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId",
                table: "Districts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_QuarterId",
                table: "Districts",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_TownId",
                table: "Districts",
                column: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Quarters_QuarterId",
                table: "Districts",
                column: "QuarterId",
                principalTable: "Quarters",
                principalColumn: "QuarterId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Towns_TownId",
                table: "Districts",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "TownId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Quarters_Cities_CityId",
                table: "Quarters",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Quarters_Towns_TownId",
                table: "Quarters",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "TownId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Towns_Cities_CityId",
                table: "Towns",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Cities_CityId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Quarters_QuarterId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Towns_TownId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Quarters_Cities_CityId",
                table: "Quarters");

            migrationBuilder.DropForeignKey(
                name: "FK_Quarters_Towns_TownId",
                table: "Quarters");

            migrationBuilder.DropForeignKey(
                name: "FK_Towns_Cities_CityId",
                table: "Towns");

            migrationBuilder.DropIndex(
                name: "IX_Towns_CityId",
                table: "Towns");

            migrationBuilder.DropIndex(
                name: "IX_Quarters_CityId",
                table: "Quarters");

            migrationBuilder.DropIndex(
                name: "IX_Quarters_TownId",
                table: "Quarters");

            migrationBuilder.DropIndex(
                name: "IX_Districts_CityId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_QuarterId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_TownId",
                table: "Districts");
        }
    }
}
