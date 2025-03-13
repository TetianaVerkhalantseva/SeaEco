using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeaEco.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIdTypeForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_ansatte1",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_ansatte2",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_ansatte3",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_ansatte4",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_prosjekt_ansatte5",
                table: "b_prosjekt");

            migrationBuilder.DropForeignKey(
                name: "fk_b_provetakingsplan_ansatte1",
                table: "b_provetakingsplan");

            migrationBuilder.DropForeignKey(
                name: "fk_b_provetakingsplan_ansatte2",
                table: "b_provetakingsplan");

            migrationBuilder.DropForeignKey(
                name: "fk_endringslogg_ansatte",
                table: "endringslogg");
            
            migrationBuilder.DropForeignKey(
                name: "fk_b_rapport_ansattgenerert",
                table: "b_rapport");
            
            migrationBuilder.DropForeignKey(
                name: "fk_b_rapport_ansattgodkjent",
                table: "b_rapport");

            migrationBuilder.DropForeignKey(
                name: "fk_tokens_user",
                table: "tokens");

            migrationBuilder.DropPrimaryKey(
                name: "tokens_pkey",
                table: "tokens");

            migrationBuilder.DropPrimaryKey(
                name: "ansatte_pkey",
                table: "ansatte");

            migrationBuilder.RenameTable(
                name: "tokens",
                newName: "token");

            migrationBuilder.RenameTable(
                name: "ansatte",
                newName: "user");

            migrationBuilder.RenameIndex(
                name: "UserId",
                table: "token",
                newName: "UserId");

            
            migrationBuilder.Sql("ALTER TABLE endringslogg ALTER COLUMN endretavid TYPE uuid USING endretavid::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE b_rapport ALTER COLUMN godkjentavid TYPE uuid USING godkjentavid::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE b_rapport ALTER COLUMN generertavid TYPE uuid USING generertavid::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE b_provetakingsplan ALTER COLUMN planleggerid TYPE uuid USING planleggerid::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE b_provetakingsplan ALTER COLUMN planlegger2id TYPE uuid USING planlegger2id::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansattid TYPE uuid USING ansvarligansattid::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansatt5id TYPE uuid USING ansvarligansatt5id::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansatt4id TYPE uuid USING ansvarligansatt4id::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansatt3id TYPE uuid USING ansvarligansatt3id::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansatt2id TYPE uuid USING ansvarligansatt2id::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE token ALTER COLUMN user_id TYPE uuid USING user_id::text::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"user\" ALTER COLUMN id DROP DEFAULT;");
            migrationBuilder.Sql("ALTER TABLE \"user\" ALTER COLUMN id TYPE uuid USING id::text::uuid;");

            
            migrationBuilder.AlterColumn<Guid>(
                name: "endretavid",
                table: "endringslogg",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "godkjentavid",
                table: "b_rapport",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "generertavid",
                table: "b_rapport",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "planleggerid",
                table: "b_provetakingsplan",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "planlegger2id",
                table: "b_provetakingsplan",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ansvarligansattid",
                table: "b_prosjekt",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "ansvarligansatt5id",
                table: "b_prosjekt",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ansvarligansatt4id",
                table: "b_prosjekt",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ansvarligansatt3id",
                table: "b_prosjekt",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ansvarligansatt2id",
                table: "b_prosjekt",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "token",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "user",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('ansatte_brukerid_seq'::regclass)");

            migrationBuilder.AddPrimaryKey(
                name: "token_pkey",
                table: "token",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "user_pkey",
                table: "user",
                column: "id");

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
                name: "fk_endringslogg_user",
                table: "endringslogg",
                column: "endretavid",
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
                name: "fk_token_user",
                table: "token",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "fk_endringslogg_user",
                table: "endringslogg");

            migrationBuilder.DropForeignKey(
                name: "fk_b_rapport_ansattgenerert",
                table: "b_rapport");

            migrationBuilder.DropForeignKey(
                name: "fk_b_rapport_ansattgodkjent",
                table: "b_rapport");

            migrationBuilder.DropForeignKey(
                name: "fk_token_user",
                table: "token");
            
            migrationBuilder.DropPrimaryKey(
                name: "user_pkey",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "token_pkey",
                table: "token");
            
            migrationBuilder.RenameTable(
                name: "user",
                newName: "ansatte");

            migrationBuilder.RenameTable(
                name: "token",
                newName: "tokens");

            migrationBuilder.RenameIndex(
                name: "UserId",
                table: "tokens",
                newName: "UserId");
            
            migrationBuilder.Sql("ALTER TABLE endringslogg ALTER COLUMN endretavid TYPE integer USING endretavid::integer;");
            migrationBuilder.Sql("ALTER TABLE b_rapport ALTER COLUMN godkjentavid TYPE integer USING godkjentavid::integer;");
            migrationBuilder.Sql("ALTER TABLE b_rapport ALTER COLUMN generertavid TYPE integer USING generertavid::integer;");
            migrationBuilder.Sql("ALTER TABLE b_provetakingsplan ALTER COLUMN planleggerid TYPE integer USING planleggerid::integer;");
            migrationBuilder.Sql("ALTER TABLE b_provetakingsplan ALTER COLUMN planlegger2id TYPE integer USING planlegger2id::integer;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansattid TYPE integer USING ansvarligansattid::integer;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansatt5id TYPE integer USING ansvarligansatt5id::integer;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansatt4id TYPE integer USING ansvarligansatt4id::integer;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansatt3id TYPE integer USING ansvarligansatt3id::integer;");
            migrationBuilder.Sql("ALTER TABLE b_prosjekt ALTER COLUMN ansvarligansatt2id TYPE integer USING ansvarligansatt2id::integer;");
            migrationBuilder.Sql("ALTER TABLE tokens ALTER COLUMN user_id TYPE integer USING user_id::integer;");
            migrationBuilder.Sql("ALTER TABLE ansatte ALTER COLUMN id TYPE integer USING id::integer;");
            migrationBuilder.Sql("ALTER TABLE ansatte ALTER COLUMN id SET DEFAULT nextval('ansatte_brukerid_seq'::regclass);");
            
            migrationBuilder.AlterColumn<int>(
                name: "endretavid",
                table: "endringslogg",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "godkjentavid",
                table: "b_rapport",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "generertavid",
                table: "b_rapport",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "planleggerid",
                table: "b_provetakingsplan",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "planlegger2id",
                table: "b_provetakingsplan",
                type: "integer",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ansvarligansattid",
                table: "b_prosjekt",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "tokens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "ansatte",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('ansatte_brukerid_seq'::regclass)",
                oldClrType: typeof(Guid),
                oldType: "uuid");
            
            migrationBuilder.AddPrimaryKey(
                name: "ansatte_pkey",
                table: "ansatte",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "tokens_pkey",
                table: "tokens",
                column: "id");
            
            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_ansatte1",
                table: "b_prosjekt",
                column: "ansvarligansattid",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_ansatte2",
                table: "b_prosjekt",
                column: "ansvarligansatt2id",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_ansatte3",
                table: "b_prosjekt",
                column: "ansvarligansatt3id",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_ansatte4",
                table: "b_prosjekt",
                column: "ansvarligansatt4id",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_prosjekt_ansatte5",
                table: "b_prosjekt",
                column: "ansvarligansatt5id",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_provetakingsplan_ansatte1",
                table: "b_provetakingsplan",
                column: "planleggerid",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_provetakingsplan_ansatte2",
                table: "b_provetakingsplan",
                column: "planlegger2id",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_endringslogg_ansatte",
                table: "endringslogg",
                column: "endretavid",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_rapport_ansattgenerert",
                table: "b_rapport",
                column: "generertavid",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_b_rapport_ansattgodkjent",
                table: "b_rapport",
                column: "godkjentavid",
                principalTable: "ansatte",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_tokens_user",
                table: "tokens",
                column: "user_id",
                principalTable: "ansatte",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

    }
}
