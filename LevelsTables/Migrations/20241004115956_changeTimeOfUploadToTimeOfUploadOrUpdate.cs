using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelsTables.Migrations
{
    /// <inheritdoc />
    public partial class changeTimeOfUploadToTimeOfUploadOrUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timeOfUpload",
                table: "Calibrations",
                newName: "timeOfUploadOrUpdate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timeOfUploadOrUpdate",
                table: "Calibrations",
                newName: "timeOfUpload");
        }
    }
}
