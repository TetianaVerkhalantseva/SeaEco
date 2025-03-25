using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaEco.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnnecessaryPropertiesAtKundeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fylke",
                table: "kunde");

            migrationBuilder.DropColumn(
                name: "kommune",
                table: "kunde");

            migrationBuilder.DropColumn(
                name: "orgnr",
                table: "kunde");

            migrationBuilder.DropColumn(
                name: "postadresse",
                table: "kunde");

            migrationBuilder.RenameColumn(
                name: "Telefonnummer",
                table: "kunde",
                newName: "telefonnummer");

            migrationBuilder.AlterColumn<string>(
                name: "telefonnummer",
                table: "kunde",
                type: "character varying(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "telefonnummer",
                table: "kunde",
                newName: "Telefonnummer");

            migrationBuilder.AlterColumn<string>(
                name: "Telefonnummer",
                table: "kunde",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(45)",
                oldMaxLength: 45);

            migrationBuilder.AddColumn<string>(
                name: "fylke",
                table: "kunde",
                type: "character varying(45)",
                maxLength: 45,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "kommune",
                table: "kunde",
                type: "character varying(45)",
                maxLength: 45,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "orgnr",
                table: "kunde",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "postadresse",
                table: "kunde",
                type: "character varying(45)",
                maxLength: 45,
                nullable: false,
                defaultValue: "");
        }
    }
}
