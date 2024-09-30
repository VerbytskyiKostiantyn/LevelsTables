using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelsTables.Migrations
{
    /// <inheritdoc />
    public partial class crateDbAddCalibrationsTanks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    MaxVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Product_zero = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Water_zero = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Alert_Level = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FuelID = table.Column<int>(type: "int", nullable: true),
                    StationID = table.Column<int>(type: "int", nullable: true),
                    TankNumber = table.Column<int>(type: "int", nullable: false),
                    TankUID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProbeSerial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Probetype = table.Column<int>(type: "int", nullable: true),
                    ExternalProbeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tanks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calibrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TankId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    modificator = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ratio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calibrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calibrations_Tanks_TankId",
                        column: x => x.TankId,
                        principalTable: "Tanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calibrations_TankId",
                table: "Calibrations",
                column: "TankId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calibrations");

            migrationBuilder.DropTable(
                name: "Tanks");
        }
    }
}
