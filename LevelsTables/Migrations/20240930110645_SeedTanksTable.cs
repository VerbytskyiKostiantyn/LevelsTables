using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LevelsTables.Migrations
{
    /// <inheritdoc />
    public partial class SeedTanksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tanks",
                columns: new[] { "Id", "Active", "Address", "Alert_Level", "Description", "ExternalProbeId", "FuelID", "MaxVolume", "Name", "ProbeSerial", "Probetype", "Product_zero", "StationID", "TankNumber", "TankUID", "Water_zero" },
                values: new object[,]
                {
                    { 1, true, "Вулиця 1", 75000m, "Основний резервуар для зберігання продуктів 1", "PROB001", 1, 500000m, "Резервуар 1", "PR001", 0, 20000m, 101, 1, "T001", 15000m },
                    { 2, false, "Вулиця 2", 60000m, "Допоміжний резервуар для зберігання продуктів Б", "PROB002", 2, 300000m, "Резервуар 2", "PR002", 1, 15000m, 102, 2, "T002", 10000m },
                    { 3, true, "Вулиця 3", 80000m, "Експериментальний резервуар для тестування нових продуктів", "PROB003", 3, 400000m, "Резервуар 3", "PR003", 2, 25000m, 103, 3, "T003", 18000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tanks",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
