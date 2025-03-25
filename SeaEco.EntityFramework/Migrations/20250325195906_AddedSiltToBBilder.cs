using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaEco.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedSiltToBBilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "silt",
                table: "b_bilder",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "silt",
                table: "b_bilder");
        }
    }
}
