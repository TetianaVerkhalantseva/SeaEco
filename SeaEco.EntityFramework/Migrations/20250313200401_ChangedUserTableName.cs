using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaEco.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_user1",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_user2",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_user3",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_user4",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_user5",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_provetakingsplan_user1",
                table: "b_provetakingsplan");

            migrationBuilder.DropForeignKey(
                name: "fk_b_provetakingsplan_user2",
                table: "b_provetakingsplan");

            migrationBuilder.DropForeignKey(
                name: "fk_b_rapport_ansattgenerert",
                table: "b_rapport");

            migrationBuilder.DropForeignKey(
                name: "fk_b_rapport_ansattgodkjent",
                table: "b_rapport");

            migrationBuilder.DropForeignKey(
                name: "fk_endringslogg_user",
                table: "endringslogg");

            migrationBuilder.DropForeignKey(
                name: "fk_token_user",
                table: "token");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "token",
                newName: "bruker_id");

            migrationBuilder.CreateTable(
                name: "bruker",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fornavn = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    etternavn = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    epost = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    passord_hash = table.Column<string>(type: "character varying", nullable: false),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false),
                    aktiv = table.Column<bool>(type: "boolean", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    salt = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("bruker_pkey", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_bruker1",
                table: "b_prosjekt",
                column: "ansvarligansattid",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_bruker2",
                table: "b_prosjekt",
                column: "ansvarligansatt2id",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_bruker3",
                table: "b_prosjekt",
                column: "ansvarligansatt3id",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_bruker4",
                table: "b_prosjekt",
                column: "ansvarligansatt4id",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_bruker5",
                table: "b_prosjekt",
                column: "ansvarligansatt5id",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_provetakingsplan_bruker1",
                table: "b_provetakingsplan",
                column: "planleggerid",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_provetakingsplan_bruker2",
                table: "b_provetakingsplan",
                column: "planlegger2id",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_rapport_ansattgenerert",
                table: "b_rapport",
                column: "generertavid",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_rapport_ansattgodkjent",
                table: "b_rapport",
                column: "godkjentavid",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_endringslogg_bruker",
                table: "endringslogg",
                column: "endretavid",
                principalTable: "bruker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_token_bruker",
                table: "token",
                column: "bruker_id",
                principalTable: "bruker",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_bruker1",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_bruker2",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_bruker3",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_bruker4",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_bruker5",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_provetakingsplan_bruker1",
                table: "b_provetakingsplan");

            migrationBuilder.DropForeignKey(
                name: "fk_b_provetakingsplan_bruker2",
                table: "b_provetakingsplan");

            migrationBuilder.DropForeignKey(
                name: "fk_b_rapport_ansattgenerert",
                table: "b_rapport");

            migrationBuilder.DropForeignKey(
                name: "fk_b_rapport_ansattgodkjent",
                table: "b_rapport");

            migrationBuilder.DropForeignKey(
                name: "fk_endringslogg_bruker",
                table: "endringslogg");

            migrationBuilder.DropForeignKey(
                name: "fk_token_bruker",
                table: "token");

            migrationBuilder.DropTable(
                name: "bruker");

            migrationBuilder.RenameColumn(
                name: "bruker_id",
                table: "token",
                newName: "user_id");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aktiv = table.Column<bool>(type: "boolean", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    epost = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    etternavn = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    fornavn = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false),
                    passord_hash = table.Column<string>(type: "character varying", nullable: false),
                    salt = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_user1",
                table: "b_prosjekt",
                column: "ansvarligansattid",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_user2",
                table: "b_prosjekt",
                column: "ansvarligansatt2id",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_user3",
                table: "b_prosjekt",
                column: "ansvarligansatt3id",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_user4",
                table: "b_prosjekt",
                column: "ansvarligansatt4id",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_user5",
                table: "b_prosjekt",
                column: "ansvarligansatt5id",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_provetakingsplan_user1",
                table: "b_provetakingsplan",
                column: "planleggerid",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_provetakingsplan_user2",
                table: "b_provetakingsplan",
                column: "planlegger2id",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_rapport_ansattgenerert",
                table: "b_rapport",
                column: "generertavid",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_rapport_ansattgodkjent",
                table: "b_rapport",
                column: "godkjentavid",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_endringslogg_user",
                table: "endringslogg",
                column: "endretavid",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_token_user",
                table: "token",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
