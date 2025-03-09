using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaEco.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AnsatteStructureChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "brukernavn",
                table: "ansatte");

            migrationBuilder.DropColumn(
                name: "roller",
                table: "ansatte");

            migrationBuilder.DropColumn(
                name: "telefonnr",
                table: "ansatte");

            migrationBuilder.RenameColumn(
                name: "fulltilgang",
                table: "ansatte",
                newName: "er_admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "er_admin",
                table: "ansatte",
                newName: "fulltilgang");

            migrationBuilder.AddColumn<string>(
                name: "brukernavn",
                table: "ansatte",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "roller",
                table: "ansatte",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "telefonnr",
                table: "ansatte",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
