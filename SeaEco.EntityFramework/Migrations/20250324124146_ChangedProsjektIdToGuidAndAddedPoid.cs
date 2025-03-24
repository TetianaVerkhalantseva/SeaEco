using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SeaEco.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ChangedProsjektIdToGuidAndAddedPoid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Sletter alle tabeller som ProsjektId av typen Int
            migrationBuilder.DropTable(name: "b_bilder");
            migrationBuilder.DropTable(name: "b_dyr");
            migrationBuilder.DropTable(name: "b_sensorisk");
            migrationBuilder.DropTable(name: "b_stasjon");
            migrationBuilder.DropTable(name: "b_rapport");
            migrationBuilder.DropTable(name: "b_provetakingsplan");
            migrationBuilder.DropTable(name: "b_prosjekt_utstyr");
            migrationBuilder.DropTable(name: "b_prosjekt");
            migrationBuilder.DropTable(name: "endringslogg");
            
            //Legger til med Guid som prosjektid
            
            migrationBuilder.CreateTable(
                name: "endringslogg",
                columns: table => new
                {
                    loggid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    stasjonsid = table.Column<int>(type: "integer", nullable: true),
                    tabellendret = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    typeending = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdiendret = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdiendretfra = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdiendrettil = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    endretavid = table.Column<Guid>(type: "uuid", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("endringslogg_pkey", x => x.loggid);
                    table.ForeignKey(
                        name: "fk_endringslogg_bruker",
                        column: x => x.endretavid,
                        principalTable: "bruker",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "b_prosjekt",
                columns: table => new
                {
                    prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    po_id = table.Column<string>(type: "text", nullable: false),
                    kundeid = table.Column<int>(type: "integer", nullable: false),
                    kundekontaktpersons = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    kundetlf = table.Column<int>(type: "integer", nullable: false),
                    kundeepost = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    lokalitetid = table.Column<int>(type: "integer", nullable: false),
                    lokalitet = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    antallstasjoner = table.Column<int>(type: "integer", nullable: false),
                    mtbtillatelse = table.Column<int>(type: "integer", nullable: false),
                    biomasse = table.Column<int>(type: "integer", nullable: false),
                    ansvarligansattid = table.Column<Guid>(type: "uuid", nullable: false),
                    ansvarligansatt2id = table.Column<Guid>(type: "uuid", nullable: true),
                    ansvarligansatt3id = table.Column<Guid>(type: "uuid", nullable: true),
                    ansvarligansatt4id = table.Column<Guid>(type: "uuid", nullable: true),
                    ansvarligansatt5id = table.Column<Guid>(type: "uuid", nullable: true),
                    planlagtfeltdato = table.Column<DateOnly>(type: "date", nullable: false),
                    merknad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    status = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_prosjekt_pkey", x => x.prosjektid);
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker1",
                        column: x => x.ansvarligansattid,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker2",
                        column: x => x.ansvarligansatt2id,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker3",
                        column: x => x.ansvarligansatt3id,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker4",
                        column: x => x.ansvarligansatt4id,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker5",
                        column: x => x.ansvarligansatt5id,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_kunde",
                        column: x => x.kundeid,
                        principalTable: "kunde",
                        principalColumn: "kundeid");
                });

            migrationBuilder.CreateTable(
                name: "b_prosjekt_utstyr",
                columns: table => new
                {
                    prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    grabbid = table.Column<int>(type: "integer", nullable: false),
                    phehmeter = table.Column<int>(type: "integer", nullable: false),
                    datokalibrert = table.Column<DateOnly>(type: "date", nullable: false),
                    silid = table.Column<int>(type: "integer", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_prosjekt_utstyr_pkey", x => x.prosjektid);
                    table.ForeignKey(
                        name: "fk_b_prosjekt_utstyr_b_prosjekt",
                        column: x => x.prosjektid,
                        principalTable: "b_prosjekt",
                        principalColumn: "prosjektid");
                });

            migrationBuilder.CreateTable(
                name: "b_provetakingsplan",
                columns: table => new
                {
                    prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    planleggerid = table.Column<Guid>(type: "uuid", nullable: false),
                    planlegger2id = table.Column<Guid>(type: "uuid", nullable: true),
                    stasjonsid = table.Column<int>(type: "integer", nullable: false),
                    planlagtfeltdato = table.Column<DateOnly>(type: "date", nullable: false),
                    planlagtdybde = table.Column<int>(type: "integer", nullable: false),
                    planlagtkordinatern = table.Column<int>(type: "integer", nullable: false),
                    planlagtkordinatero = table.Column<int>(type: "integer", nullable: false),
                    planlagtanalyser = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false, defaultValueSql: "'Parameter I, II og III'::character varying"),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_provetakingsplan_pkey", x => x.prosjektid);
                    table.ForeignKey(
                        name: "fk_b_provetakingsplan_b_prosjekt",
                        column: x => x.prosjektid,
                        principalTable: "b_prosjekt",
                        principalColumn: "prosjektid");
                    table.ForeignKey(
                        name: "fk_b_provetakingsplan_bruker1",
                        column: x => x.planleggerid,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_provetakingsplan_bruker2",
                        column: x => x.planlegger2id,
                        principalTable: "bruker",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "b_rapport",
                columns: table => new
                {
                    rapportid = table.Column<int>(type: "integer", nullable: false),
                    prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    rapporttype = table.Column<int>(type: "integer", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    generertavid = table.Column<Guid>(type: "uuid", nullable: false),
                    godkjentavid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_rapport_pkey", x => x.rapportid);
                    table.ForeignKey(
                        name: "fk_b_rapport_ansattgenerert",
                        column: x => x.generertavid,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_rapport_ansattgodkjent",
                        column: x => x.godkjentavid,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_rapport_b_prosjekt",
                        column: x => x.prosjektid,
                        principalTable: "b_prosjekt",
                        principalColumn: "prosjektid");
                });

            migrationBuilder.CreateTable(
                name: "b_stasjon",
                columns: table => new
                {
                    prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    stasjonsid = table.Column<int>(type: "integer", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    dybde = table.Column<int>(type: "integer", nullable: false),
                    kordinatern = table.Column<int>(type: "integer", nullable: false),
                    kordinatero = table.Column<int>(type: "integer", nullable: false),
                    skjovann_ph = table.Column<int>(type: "integer", nullable: false),
                    skjovann_eh = table.Column<int>(type: "integer", nullable: false),
                    skjovann_temperatur = table.Column<int>(type: "integer", nullable: false),
                    bunntype = table.Column<bool>(type: "boolean", nullable: false),
                    dyr = table.Column<bool>(type: "boolean", nullable: false),
                    antallgrabbskudd = table.Column<int>(type: "integer", nullable: false),
                    grabhastighetgodkjent = table.Column<bool>(type: "boolean", nullable: false),
                    sensoriskutfort = table.Column<bool>(type: "boolean", nullable: true),
                    bunnsammensettningid = table.Column<int>(type: "integer", nullable: false),
                    arter = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: true),
                    merknad = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: true),
                    korrigering = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: true),
                    grabbid = table.Column<int>(type: "integer", nullable: true),
                    phehmeter = table.Column<int>(type: "integer", nullable: true),
                    datokalibrert = table.Column<DateOnly>(type: "date", nullable: true),
                    silid = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_stasjon_pkey", x => new { x.prosjektid, x.stasjonsid });
                    table.ForeignKey(
                        name: "fk_b_stasjon_b_prosjekt",
                        column: x => x.prosjektid,
                        principalTable: "b_prosjekt",
                        principalColumn: "prosjektid");
                    table.ForeignKey(
                        name: "fk_b_stasjon_sys_bunsammensettning",
                        column: x => x.bunnsammensettningid,
                        principalTable: "sys_bunsammensettning",
                        principalColumn: "bunnsammensettningid");
                });

            migrationBuilder.CreateTable(
                name: "b_bilder",
                columns: table => new
                {
                    bildeid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    posisjon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    stasjonsid = table.Column<int>(type: "integer", nullable: false),
                    bilde = table.Column<byte[]>(type: "bytea", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_bilder_pkey", x => x.bildeid);
                    table.ForeignKey(
                        name: "fk_b_bilder_b_stasjon",
                        columns: x => new { x.prosjektid, x.stasjonsid },
                        principalTable: "b_stasjon",
                        principalColumns: new[] { "prosjektid", "stasjonsid" });
                });

            migrationBuilder.CreateTable(
                name: "b_dyr",
                columns: table => new
                {
                    prosjekt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stasjons_id = table.Column<int>(type: "integer", nullable: false),
                    antallpigghunder = table.Column<int>(type: "integer", nullable: true),
                    antallkrepsdyr = table.Column<int>(type: "integer", nullable: true),
                    antallskjell = table.Column<int>(type: "integer", nullable: true),
                    antallborstemark = table.Column<int>(type: "integer", nullable: true),
                    beggiota = table.Column<bool>(type: "boolean", nullable: true),
                    foor = table.Column<bool>(type: "boolean", nullable: true),
                    fekalier = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_dyr_pkey", x => new { x.prosjekt_id, x.stasjons_id });
                    table.ForeignKey(
                        name: "fk_b_dyr_b_stasjon",
                        columns: x => new { x.prosjekt_id, x.stasjons_id },
                        principalTable: "b_stasjon",
                        principalColumns: new[] { "prosjektid", "stasjonsid" });
                });

            migrationBuilder.CreateTable(
                name: "b_sensorisk",
                columns: table => new
                {
                    prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    stasjonsid = table.Column<int>(type: "integer", nullable: false),
                    prove_ph = table.Column<int>(type: "integer", nullable: false),
                    prove_eh = table.Column<int>(type: "integer", nullable: false),
                    prove_temperatur = table.Column<int>(type: "integer", nullable: false),
                    farge = table.Column<bool>(type: "boolean", nullable: false),
                    lukt = table.Column<int>(type: "integer", nullable: false),
                    konsistens = table.Column<int>(type: "integer", nullable: false),
                    grabbvolum = table.Column<int>(type: "integer", nullable: false),
                    tykkelseslamlag = table.Column<int>(type: "integer", nullable: false),
                    gassbobler = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_sensorisk_pkey", x => new { x.prosjektid, x.stasjonsid });
                    table.ForeignKey(
                        name: "fk_b_sensorisk_b_stasjon",
                        columns: x => new { x.prosjektid, x.stasjonsid },
                        principalTable: "b_stasjon",
                        principalColumns: new[] { "prosjektid", "stasjonsid" });
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_farge",
                        column: x => x.farge,
                        principalTable: "sys_farge",
                        principalColumn: "verdi");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_gassbobler",
                        column: x => x.gassbobler,
                        principalTable: "sys_gassbobler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_grabbvolum",
                        column: x => x.grabbvolum,
                        principalTable: "sys_grabbvolum",
                        principalColumn: "verdi");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_konsistens",
                        column: x => x.konsistens,
                        principalTable: "sys_konsistens",
                        principalColumn: "verdi");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_lukt",
                        column: x => x.lukt,
                        principalTable: "sys_lukt",
                        principalColumn: "verdi");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_tykkelsepaslam",
                        column: x => x.tykkelseslamlag,
                        principalTable: "sys_tykkelsepaslam",
                        principalColumn: "verdi");
                });

            migrationBuilder.CreateIndex(
                name: "IX_b_bilder_prosjektid_stasjonsid",
                table: "b_bilder",
                columns: new[] { "prosjektid", "stasjonsid" });

            migrationBuilder.CreateIndex(
                name: "IX_b_prosjekt_ansvarligansatt2id",
                table: "b_prosjekt",
                column: "ansvarligansatt2id");

            migrationBuilder.CreateIndex(
                name: "IX_b_prosjekt_ansvarligansatt3id",
                table: "b_prosjekt",
                column: "ansvarligansatt3id");

            migrationBuilder.CreateIndex(
                name: "IX_b_prosjekt_ansvarligansatt4id",
                table: "b_prosjekt",
                column: "ansvarligansatt4id");

            migrationBuilder.CreateIndex(
                name: "IX_b_prosjekt_ansvarligansatt5id",
                table: "b_prosjekt",
                column: "ansvarligansatt5id");

            migrationBuilder.CreateIndex(
                name: "IX_b_prosjekt_ansvarligansattid",
                table: "b_prosjekt",
                column: "ansvarligansattid");

            migrationBuilder.CreateIndex(
                name: "IX_b_prosjekt_kundeid",
                table: "b_prosjekt",
                column: "kundeid");

            migrationBuilder.CreateIndex(
                name: "IX_b_provetakingsplan_planlegger2id",
                table: "b_provetakingsplan",
                column: "planlegger2id");

            migrationBuilder.CreateIndex(
                name: "IX_b_provetakingsplan_planleggerid",
                table: "b_provetakingsplan",
                column: "planleggerid");

            migrationBuilder.CreateIndex(
                name: "IX_b_rapport_generertavid",
                table: "b_rapport",
                column: "generertavid");

            migrationBuilder.CreateIndex(
                name: "IX_b_rapport_godkjentavid",
                table: "b_rapport",
                column: "godkjentavid");

            migrationBuilder.CreateIndex(
                name: "IX_b_rapport_prosjektid",
                table: "b_rapport",
                column: "prosjektid");

            migrationBuilder.CreateIndex(
                name: "IX_b_sensorisk_farge",
                table: "b_sensorisk",
                column: "farge");

            migrationBuilder.CreateIndex(
                name: "IX_b_sensorisk_gassbobler",
                table: "b_sensorisk",
                column: "gassbobler");

            migrationBuilder.CreateIndex(
                name: "IX_b_sensorisk_grabbvolum",
                table: "b_sensorisk",
                column: "grabbvolum");

            migrationBuilder.CreateIndex(
                name: "IX_b_sensorisk_konsistens",
                table: "b_sensorisk",
                column: "konsistens");

            migrationBuilder.CreateIndex(
                name: "IX_b_sensorisk_lukt",
                table: "b_sensorisk",
                column: "lukt");

            migrationBuilder.CreateIndex(
                name: "IX_b_sensorisk_tykkelseslamlag",
                table: "b_sensorisk",
                column: "tykkelseslamlag");

            migrationBuilder.CreateIndex(
                name: "IX_b_stasjon_bunnsammensettningid",
                table: "b_stasjon",
                column: "bunnsammensettningid");

            migrationBuilder.CreateIndex(
                name: "IX_endringslogg_endretavid",
                table: "endringslogg",
                column: "endretavid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "b_bilder");

            migrationBuilder.DropTable(
                name: "b_dyr");

            migrationBuilder.DropTable(
                name: "b_prosjekt_utstyr");

            migrationBuilder.DropTable(
                name: "b_provetakingsplan");

            migrationBuilder.DropTable(
                name: "b_rapport");

            migrationBuilder.DropTable(
                name: "b_sensorisk");

            migrationBuilder.DropTable(
                name: "endringslogg");
            
            migrationBuilder.DropTable(
                name: "b_stasjon");
            
            migrationBuilder.DropTable(
                name: "b_prosjekt");
            
            
            migrationBuilder.CreateTable(
                name: "b_prosjekt",
                columns: table => new
                {
                    prosjektid = table.Column<int>(type: "integer", nullable: false),
                    kundeid = table.Column<int>(type: "integer", nullable: false),
                    kundekontaktpersons = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    kundetlf = table.Column<int>(type: "integer", nullable: false),
                    kundeepost = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    lokalitetid = table.Column<int>(type: "integer", nullable: false),
                    lokalitet = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    antallstasjoner = table.Column<int>(type: "integer", nullable: false),
                    mtbtillatelse = table.Column<int>(type: "integer", nullable: false),
                    biomasse = table.Column<int>(type: "integer", nullable: false),
                    ansvarligansattid = table.Column<Guid>(type: "uuid", nullable: false),
                    ansvarligansatt2id = table.Column<Guid>(type: "uuid", nullable: true),
                    ansvarligansatt3id = table.Column<Guid>(type: "uuid", nullable: true),
                    ansvarligansatt4id = table.Column<Guid>(type: "uuid", nullable: true),
                    ansvarligansatt5id = table.Column<Guid>(type: "uuid", nullable: true),
                    planlagtfeltdato = table.Column<DateOnly>(type: "date", nullable: false),
                    merknad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    status = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_prosjekt_pkey", x => x.prosjektid);
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker1",
                        column: x => x.ansvarligansattid,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker2",
                        column: x => x.ansvarligansatt2id,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker3",
                        column: x => x.ansvarligansatt3id,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker4",
                        column: x => x.ansvarligansatt4id,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_bruker5",
                        column: x => x.ansvarligansatt5id,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_prosjekt_kunde",
                        column: x => x.kundeid,
                        principalTable: "kunde",
                        principalColumn: "kundeid");
                });

            migrationBuilder.CreateTable(
                name: "endringslogg",
                columns: table => new
                {
                    loggid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prosjektid = table.Column<int>(type: "integer", nullable: false),
                    stasjonsid = table.Column<int>(type: "integer", nullable: true),
                    tabellendret = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    typeending = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdiendret = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdiendretfra = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdiendrettil = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    endretavid = table.Column<Guid>(type: "uuid", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("endringslogg_pkey", x => x.loggid);
                    table.ForeignKey(
                        name: "fk_endringslogg_bruker",
                        column: x => x.endretavid,
                        principalTable: "bruker",
                        principalColumn: "id");
                });
            
            migrationBuilder.CreateTable(
                name: "b_prosjekt_utstyr",
                columns: table => new
                {
                    prosjektid = table.Column<int>(type: "integer", nullable: false),
                    grabbid = table.Column<int>(type: "integer", nullable: false),
                    phehmeter = table.Column<int>(type: "integer", nullable: false),
                    datokalibrert = table.Column<DateOnly>(type: "date", nullable: false),
                    silid = table.Column<int>(type: "integer", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_prosjekt_utstyr_pkey", x => x.prosjektid);
                    table.ForeignKey(
                        name: "fk_b_prosjekt_utstyr_b_prosjekt",
                        column: x => x.prosjektid,
                        principalTable: "b_prosjekt",
                        principalColumn: "prosjektid");
                });

            migrationBuilder.CreateTable(
                name: "b_provetakingsplan",
                columns: table => new
                {
                    prosjektid = table.Column<int>(type: "integer", nullable: false),
                    planleggerid = table.Column<Guid>(type: "uuid", nullable: false),
                    planlegger2id = table.Column<Guid>(type: "uuid", nullable: true),
                    stasjonsid = table.Column<int>(type: "integer", nullable: false),
                    planlagtfeltdato = table.Column<DateOnly>(type: "date", nullable: false),
                    planlagtdybde = table.Column<int>(type: "integer", nullable: false),
                    planlagtkordinatern = table.Column<int>(type: "integer", nullable: false),
                    planlagtkordinatero = table.Column<int>(type: "integer", nullable: false),
                    planlagtanalyser = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false, defaultValueSql: "'Parameter I, II og III'::character varying"),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_provetakingsplan_pkey", x => x.prosjektid);
                    table.ForeignKey(
                        name: "fk_b_provetakingsplan_b_prosjekt",
                        column: x => x.prosjektid,
                        principalTable: "b_prosjekt",
                        principalColumn: "prosjektid");
                    table.ForeignKey(
                        name: "fk_b_provetakingsplan_bruker1",
                        column: x => x.planleggerid,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_provetakingsplan_bruker2",
                        column: x => x.planlegger2id,
                        principalTable: "bruker",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "b_rapport",
                columns: table => new
                {
                    rapportid = table.Column<int>(type: "integer", nullable: false),
                    prosjektid = table.Column<int>(type: "integer", nullable: false),
                    rapporttype = table.Column<int>(type: "integer", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    generertavid = table.Column<Guid>(type: "uuid", nullable: false),
                    godkjentavid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_rapport_pkey", x => x.rapportid);
                    table.ForeignKey(
                        name: "fk_b_rapport_ansattgenerert",
                        column: x => x.generertavid,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_rapport_ansattgodkjent",
                        column: x => x.godkjentavid,
                        principalTable: "bruker",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_rapport_b_prosjekt",
                        column: x => x.prosjektid,
                        principalTable: "b_prosjekt",
                        principalColumn: "prosjektid");
                });

            migrationBuilder.CreateTable(
                name: "b_stasjon",
                columns: table => new
                {
                    prosjektid = table.Column<int>(type: "integer", nullable: false),
                    stasjonsid = table.Column<int>(type: "integer", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    dybde = table.Column<int>(type: "integer", nullable: false),
                    kordinatern = table.Column<int>(type: "integer", nullable: false),
                    kordinatero = table.Column<int>(type: "integer", nullable: false),
                    skjovann_ph = table.Column<int>(type: "integer", nullable: false),
                    skjovann_eh = table.Column<int>(type: "integer", nullable: false),
                    skjovann_temperatur = table.Column<int>(type: "integer", nullable: false),
                    bunntype = table.Column<bool>(type: "boolean", nullable: false),
                    dyr = table.Column<bool>(type: "boolean", nullable: false),
                    antallgrabbskudd = table.Column<int>(type: "integer", nullable: false),
                    grabhastighetgodkjent = table.Column<bool>(type: "boolean", nullable: false),
                    sensoriskutfort = table.Column<bool>(type: "boolean", nullable: true),
                    bunnsammensettningid = table.Column<int>(type: "integer", nullable: false),
                    arter = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: true),
                    merknad = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: true),
                    korrigering = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: true),
                    grabbid = table.Column<int>(type: "integer", nullable: true),
                    phehmeter = table.Column<int>(type: "integer", nullable: true),
                    datokalibrert = table.Column<DateOnly>(type: "date", nullable: true),
                    silid = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_stasjon_pkey", x => new { x.prosjektid, x.stasjonsid });
                    table.ForeignKey(
                        name: "fk_b_stasjon_b_prosjekt",
                        column: x => x.prosjektid,
                        principalTable: "b_prosjekt",
                        principalColumn: "prosjektid");
                    table.ForeignKey(
                        name: "fk_b_stasjon_sys_bunsammensettning",
                        column: x => x.bunnsammensettningid,
                        principalTable: "sys_bunsammensettning",
                        principalColumn: "bunnsammensettningid");
                });

            migrationBuilder.CreateTable(
                name: "b_bilder",
                columns: table => new
                {
                    bildeid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    posisjon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    prosjektid = table.Column<int>(type: "integer", nullable: false),
                    stasjonsid = table.Column<int>(type: "integer", nullable: false),
                    bilde = table.Column<byte[]>(type: "bytea", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_bilder_pkey", x => x.bildeid);
                    table.ForeignKey(
                        name: "fk_b_bilder_b_stasjon",
                        columns: x => new { x.prosjektid, x.stasjonsid },
                        principalTable: "b_stasjon",
                        principalColumns: new[] { "prosjektid", "stasjonsid" });
                });

            migrationBuilder.CreateTable(
                name: "b_dyr",
                columns: table => new
                {
                    prosjekt_id = table.Column<int>(type: "integer", nullable: false),
                    stasjons_id = table.Column<int>(type: "integer", nullable: false),
                    antallpigghunder = table.Column<int>(type: "integer", nullable: true),
                    antallkrepsdyr = table.Column<int>(type: "integer", nullable: true),
                    antallskjell = table.Column<int>(type: "integer", nullable: true),
                    antallborstemark = table.Column<int>(type: "integer", nullable: true),
                    beggiota = table.Column<bool>(type: "boolean", nullable: true),
                    foor = table.Column<bool>(type: "boolean", nullable: true),
                    fekalier = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_dyr_pkey", x => new { x.prosjekt_id, x.stasjons_id });
                    table.ForeignKey(
                        name: "fk_b_dyr_b_stasjon",
                        columns: x => new { x.prosjekt_id, x.stasjons_id },
                        principalTable: "b_stasjon",
                        principalColumns: new[] { "prosjektid", "stasjonsid" });
                });

            migrationBuilder.CreateTable(
                name: "b_sensorisk",
                columns: table => new
                {
                    prosjektid = table.Column<int>(type: "integer", nullable: false),
                    stasjonsid = table.Column<int>(type: "integer", nullable: false),
                    prove_ph = table.Column<int>(type: "integer", nullable: false),
                    prove_eh = table.Column<int>(type: "integer", nullable: false),
                    prove_temperatur = table.Column<int>(type: "integer", nullable: false),
                    farge = table.Column<bool>(type: "boolean", nullable: false),
                    lukt = table.Column<int>(type: "integer", nullable: false),
                    konsistens = table.Column<int>(type: "integer", nullable: false),
                    grabbvolum = table.Column<int>(type: "integer", nullable: false),
                    tykkelseslamlag = table.Column<int>(type: "integer", nullable: false),
                    gassbobler = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_sensorisk_pkey", x => new { x.prosjektid, x.stasjonsid });
                    table.ForeignKey(
                        name: "fk_b_sensorisk_b_stasjon",
                        columns: x => new { x.prosjektid, x.stasjonsid },
                        principalTable: "b_stasjon",
                        principalColumns: new[] { "prosjektid", "stasjonsid" });
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_farge",
                        column: x => x.farge,
                        principalTable: "sys_farge",
                        principalColumn: "verdi");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_gassbobler",
                        column: x => x.gassbobler,
                        principalTable: "sys_gassbobler",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_grabbvolum",
                        column: x => x.grabbvolum,
                        principalTable: "sys_grabbvolum",
                        principalColumn: "verdi");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_konsistens",
                        column: x => x.konsistens,
                        principalTable: "sys_konsistens",
                        principalColumn: "verdi");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_lukt",
                        column: x => x.lukt,
                        principalTable: "sys_lukt",
                        principalColumn: "verdi");
                    table.ForeignKey(
                        name: "fk_b_sensorisk_sys_tykkelsepaslam",
                        column: x => x.tykkelseslamlag,
                        principalTable: "sys_tykkelsepaslam",
                        principalColumn: "verdi");
                });
        }
    }
}
