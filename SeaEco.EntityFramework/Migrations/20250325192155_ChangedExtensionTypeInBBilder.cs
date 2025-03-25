using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaEco.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ChangedExtensionTypeInBBilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "extension",
                table: "b_bilder",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "extension",
                table: "b_bilder",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
