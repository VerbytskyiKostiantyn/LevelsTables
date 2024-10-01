using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelsTables.Migrations
{
    /// <inheritdoc />
    public partial class updateDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dateTime",
                table: "Calibrations",
                newName: "timeOfUpload");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timeOfUpload",
                table: "Calibrations",
                newName: "dateTime");
        }
    }
}
