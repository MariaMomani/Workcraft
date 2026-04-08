using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workcraft.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "position",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "position",
                table: "AspNetUsers");
        }
    }
}
