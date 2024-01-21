using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmanuelCegidTest.Migrations
{
    /// <inheritdoc />
    public partial class SalesUnitDel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesItems_SalesUnit_SalesUnitId",
                table: "SalesItems");

            migrationBuilder.DropTable(
                name: "SalesUnit");

            migrationBuilder.DropIndex(
                name: "IX_SalesItems_SalesUnitId",
                table: "SalesItems");

            migrationBuilder.DropColumn(
                name: "SalesUnitId",
                table: "SalesItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "SalesItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "SalesUnit",
                table: "SalesItems",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesUnit",
                table: "SalesItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "SalesItems",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "SalesUnitId",
                table: "SalesItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SalesUnit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesUnit", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesItems_SalesUnitId",
                table: "SalesItems",
                column: "SalesUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesItems_SalesUnit_SalesUnitId",
                table: "SalesItems",
                column: "SalesUnitId",
                principalTable: "SalesUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
