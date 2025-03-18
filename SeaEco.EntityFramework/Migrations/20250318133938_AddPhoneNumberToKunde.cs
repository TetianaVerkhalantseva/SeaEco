using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaEco.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneNumberToKunde : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefonnummer",
                table: "kunde",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefonnummer",
                table: "kunde");
        }
    }
}
