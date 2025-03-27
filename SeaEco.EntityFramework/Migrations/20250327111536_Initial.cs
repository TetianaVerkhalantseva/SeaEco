using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SeaEco.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    salt = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("bruker_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kunde",
                columns: table => new
                {
                    kundeid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    oppdragsgiver = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    kontaktperson = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    telefonnummer = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("kunde_pkey", x => x.kundeid);
                });

            migrationBuilder.CreateTable(
                name: "revisjonslogg",
                columns: table => new
                {
                    revisjonsid = table.Column<int>(type: "integer", nullable: false),
                    revisjonskommentar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    gjeldenderevisjon = table.Column<bool>(type: "boolean", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("revisjonslogg_pkey", x => x.revisjonsid);
                });

            migrationBuilder.CreateTable(
                name: "sys_arter",
                columns: table => new
                {
                    artid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    artsforkortelse = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    artsnavn = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sys_arter_pkey", x => x.artid);
                });

            migrationBuilder.CreateTable(
                name: "sys_bunsammensettning",
                columns: table => new
                {
                    bunnsammensettningid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sand = table.Column<bool>(type: "boolean", nullable: true),
                    leire = table.Column<bool>(type: "boolean", nullable: true),
                    silt = table.Column<bool>(type: "boolean", nullable: true),
                    grus = table.Column<bool>(type: "boolean", nullable: true),
                    skjellsand = table.Column<bool>(type: "boolean", nullable: true),
                    steinbunn = table.Column<bool>(type: "boolean", nullable: true),
                    fjellbunn = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sys_bunsammensettning_pkey", x => x.bunnsammensettningid);
                });

            migrationBuilder.CreateTable(
                name: "sys_farge",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    beskrivelse = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sys_farge_pkey", x => x.id);
                    table.UniqueConstraint("AK_sys_farge_verdi", x => x.verdi);
                });

            migrationBuilder.CreateTable(
                name: "sys_gassbobler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    beskrivelse = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdi = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sys_gassbobler_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_grabbvolum",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    beskrivelse = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdi = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sys_grabbvolum_pkey", x => x.id);
                    table.UniqueConstraint("AK_sys_grabbvolum_verdi", x => x.verdi);
                });

            migrationBuilder.CreateTable(
                name: "sys_konsistens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    beskrivelse = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdi = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sys_konsistens_pkey", x => x.id);
                    table.UniqueConstraint("AK_sys_konsistens_verdi", x => x.verdi);
                });

            migrationBuilder.CreateTable(
                name: "sys_lukt",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    beskrivelse = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdi = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sys_lukt_pkey", x => x.id);
                    table.UniqueConstraint("AK_sys_lukt_verdi", x => x.verdi);
                });

            migrationBuilder.CreateTable(
                name: "sys_merknad",
                columns: table => new
                {
                    merknadid = table.Column<int>(type: "integer", nullable: false),
                    forkortelse = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    beskrivelse = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sys_merknad_pkey", x => x.merknadid);
                });

            migrationBuilder.CreateTable(
                name: "sys_tykkelsepaslam",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    beskrivelse = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    verdi = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sys_tykkelsepaslam_pkey", x => x.id);
                    table.UniqueConstraint("AK_sys_tykkelsepaslam_verdi", x => x.verdi);
                });

            migrationBuilder.CreateTable(
                name: "endringslogg",
                columns: table => new
                {
                    loggid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    stasjonsid = table.Column<Guid>(type: "uuid", nullable: true),
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
                name: "token",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    used_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    expired_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    bruker_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("token_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_token_bruker",
                        column: x => x.bruker_id,
                        principalTable: "bruker",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    stasjonsid = table.Column<Guid>(type: "uuid", nullable: false),
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
                    stasjonsid = table.Column<Guid>(type: "uuid", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    nummer = table.Column<int>(type: "integer", nullable: false),
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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    posisjon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    silt = table.Column<bool>(type: "boolean", nullable: false),
                    extension = table.Column<string>(type: "text", nullable: false),
                    Prosjektid = table.Column<Guid>(type: "uuid", nullable: false),
                    stasjonsid = table.Column<Guid>(type: "uuid", nullable: false),
                    datoregistrert = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("b_bilder_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_b_bilder_b_stasjon",
                        columns: x => new { x.Prosjektid, x.stasjonsid },
                        principalTable: "b_stasjon",
                        principalColumns: new[] { "prosjektid", "stasjonsid" });
                });

            migrationBuilder.CreateTable(
                name: "b_dyr",
                columns: table => new
                {
                    prosjekt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stasjons_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    stasjonsid = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "IX_b_bilder_Prosjektid_stasjonsid",
                table: "b_bilder",
                columns: new[] { "Prosjektid", "stasjonsid" });

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

            migrationBuilder.CreateIndex(
                name: "sys_farge_verdi_key",
                table: "sys_farge",
                column: "verdi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "sys_grabbvolum_verdi_key",
                table: "sys_grabbvolum",
                column: "verdi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "sys_konsistens_verdi_key",
                table: "sys_konsistens",
                column: "verdi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "sys_lukt_verdi_key",
                table: "sys_lukt",
                column: "verdi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "sys_tykkelsepaslam_verdi_key",
                table: "sys_tykkelsepaslam",
                column: "verdi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_token_bruker_id",
                table: "token",
                column: "bruker_id");
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
                name: "revisjonslogg");

            migrationBuilder.DropTable(
                name: "sys_arter");

            migrationBuilder.DropTable(
                name: "sys_merknad");

            migrationBuilder.DropTable(
                name: "token");

            migrationBuilder.DropTable(
                name: "b_stasjon");

            migrationBuilder.DropTable(
                name: "sys_farge");

            migrationBuilder.DropTable(
                name: "sys_gassbobler");

            migrationBuilder.DropTable(
                name: "sys_grabbvolum");

            migrationBuilder.DropTable(
                name: "sys_konsistens");

            migrationBuilder.DropTable(
                name: "sys_lukt");

            migrationBuilder.DropTable(
                name: "sys_tykkelsepaslam");

            migrationBuilder.DropTable(
                name: "b_prosjekt");

            migrationBuilder.DropTable(
                name: "sys_bunsammensettning");

            migrationBuilder.DropTable(
                name: "bruker");

            migrationBuilder.DropTable(
                name: "kunde");
        }
    }
}
