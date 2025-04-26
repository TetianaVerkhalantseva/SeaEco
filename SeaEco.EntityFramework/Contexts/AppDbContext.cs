using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.EntityFramework.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }
    
    public virtual DbSet<Bruker> Brukers { get; set; }

    public virtual DbSet<BBilder> BBilders { get; set; }

    public virtual DbSet<BDyr> BDyrs { get; set; }

    public virtual DbSet<BProsjekt> BProsjekts { get; set; }

    public virtual DbSet<BProsjektUtstyr> BProsjektUtstyrs { get; set; }

    public virtual DbSet<BProvetakingsplan> BProvetakingsplans { get; set; }

    public virtual DbSet<BRapport> BRapports { get; set; }

    public virtual DbSet<BSensorisk> BSensorisks { get; set; }

    public virtual DbSet<BStasjon> BStasjons { get; set; }

    public virtual DbSet<Endringslogg> Endringsloggs { get; set; }

    public virtual DbSet<Kunde> Kundes { get; set; }

    public virtual DbSet<Revisjonslogg> Revisjonsloggs { get; set; }

    public virtual DbSet<SysArter> SysArters { get; set; }

    public virtual DbSet<SysBunsammensettning> SysBunsammensettnings { get; set; }

    public virtual DbSet<SysFarge> SysFarges { get; set; }

    public virtual DbSet<SysGassbobler> SysGassboblers { get; set; }

    public virtual DbSet<SysGrabbvolum> SysGrabbvolums { get; set; }

    public virtual DbSet<SysKonsisten> SysKonsistens { get; set; }

    public virtual DbSet<SysLukt> SysLukts { get; set; }

    public virtual DbSet<SysMerknad> SysMerknads { get; set; }

    public virtual DbSet<SysTykkelsepaslam> SysTykkelsepaslams { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=10.239.120.212;Database=seaeco;Port=5432;username=admin;password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bruker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bruker_pkey");

            entity.ToTable("bruker");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Aktiv).HasColumnName("aktiv");
            entity.Property(e => e.Datoregistrert)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.Epost)
                .HasMaxLength(45)
                .HasColumnName("epost");
            entity.Property(e => e.Etternavn)
                .HasMaxLength(45)
                .HasColumnName("etternavn");
            entity.Property(e => e.Fornavn)
                .HasMaxLength(45)
                .HasColumnName("fornavn");
            entity.Property(e => e.IsAdmin).HasColumnName("is_admin");
            entity.Property(e => e.PassordHash)
                .HasColumnType("character varying")
                .HasColumnName("passord_hash");
            entity.Property(e => e.Salt)
                .HasColumnType("bytea")
                .HasColumnName("salt");
        });

        modelBuilder.Entity<BBilder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("b_bilder_pkey");

            entity.ToTable("b_bilder");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Silt).HasColumnName("silt");
            entity.Property(e => e.Extension).HasColumnName("extension");
            entity.Property(e => e.Datoregistrert)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.Posisjon)
                .HasMaxLength(255)
                .HasColumnName("posisjon");
            entity.Property(e => e.Stasjonsid).HasColumnName("stasjonsid");

            entity.HasOne(d => d.BStasjon).WithMany(p => p.BBilders)
                .HasForeignKey(d => new { d.Prosjektid, d.Stasjonsid })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_bilder_b_stasjon");
        });

        modelBuilder.Entity<BDyr>(entity =>
        {
            entity.HasKey(e => new { e.ProsjektId, e.StasjonsId }).HasName("b_dyr_pkey");

            entity.ToTable("b_dyr");

            entity.Property(e => e.ProsjektId).HasColumnName("prosjekt_id");
            entity.Property(e => e.StasjonsId).HasColumnName("stasjons_id");
            entity.Property(e => e.Antallborstemark).HasColumnName("antallborstemark");
            entity.Property(e => e.Antallkrepsdyr).HasColumnName("antallkrepsdyr");
            entity.Property(e => e.Antallpigghunder).HasColumnName("antallpigghunder");
            entity.Property(e => e.Antallskjell).HasColumnName("antallskjell");
            entity.Property(e => e.Beggiota).HasColumnName("beggiota");
            entity.Property(e => e.Fekalier).HasColumnName("fekalier");
            entity.Property(e => e.Foor).HasColumnName("foor");

            entity.HasOne(d => d.BStasjon).WithOne(p => p.BDyr)
                .HasForeignKey<BDyr>(d => new { d.ProsjektId, d.StasjonsId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_dyr_b_stasjon");
        });

        modelBuilder.Entity<BProsjekt>(entity =>
        {
            entity.HasKey(e => e.Prosjektid).HasName("b_prosjekt_pkey");

            entity.ToTable("b_prosjekt");

            entity.Property(e => e.Prosjektid)
                .ValueGeneratedOnAdd()
                .HasColumnName("prosjektid");
            entity.Property(e => e.PoId).HasColumnName("po_id");
            entity.Property(e => e.Ansvarligansatt2id).HasColumnName("ansvarligansatt2id");
            entity.Property(e => e.Ansvarligansatt3id).HasColumnName("ansvarligansatt3id");
            entity.Property(e => e.Ansvarligansatt4id).HasColumnName("ansvarligansatt4id");
            entity.Property(e => e.Ansvarligansatt5id).HasColumnName("ansvarligansatt5id");
            entity.Property(e => e.Ansvarligansattid).HasColumnName("ansvarligansattid");
            entity.Property(e => e.Antallstasjoner).HasColumnName("antallstasjoner");
            entity.Property(e => e.Biomasse).HasColumnName("biomasse");
            entity.Property(e => e.Datoregistrert)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.Kundeepost)
                .HasMaxLength(45)
                .HasColumnName("kundeepost");
            entity.Property(e => e.Kundeid).HasColumnName("kundeid");
            entity.Property(e => e.Kundekontaktpersons)
                .HasMaxLength(45)
                .HasColumnName("kundekontaktpersons");
            entity.Property(e => e.Kundetlf).HasColumnName("kundetlf");
            entity.Property(e => e.Lokalitet)
                .HasMaxLength(45)
                .HasColumnName("lokalitet");
            entity.Property(e => e.Lokalitetid).HasColumnName("lokalitetid");
            entity.Property(e => e.Merknad)
                .HasMaxLength(200)
                .HasColumnName("merknad");
            entity.Property(e => e.Mtbtillatelse).HasColumnName("mtbtillatelse");
            entity.Property(e => e.Planlagtfeltdato).HasColumnName("planlagtfeltdato");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");

            entity.HasOne(d => d.Ansvarligansatt2).WithMany(p => p.BProsjektAnsvarligansatt2s)
                .HasForeignKey(d => d.Ansvarligansatt2id)
                .HasConstraintName("fk_b_prosjekt_bruker2");

            entity.HasOne(d => d.Ansvarligansatt3).WithMany(p => p.BProsjektAnsvarligansatt3s)
                .HasForeignKey(d => d.Ansvarligansatt3id)
                .HasConstraintName("fk_b_prosjekt_bruker3");

            entity.HasOne(d => d.Ansvarligansatt4).WithMany(p => p.BProsjektAnsvarligansatt4s)
                .HasForeignKey(d => d.Ansvarligansatt4id)
                .HasConstraintName("fk_b_prosjekt_bruker4");

            entity.HasOne(d => d.Ansvarligansatt5).WithMany(p => p.BProsjektAnsvarligansatt5s)
                .HasForeignKey(d => d.Ansvarligansatt5id)
                .HasConstraintName("fk_b_prosjekt_bruker5");

            entity.HasOne(d => d.Ansvarligansatt).WithMany(p => p.BProsjektAnsvarligansatts)
                .HasForeignKey(d => d.Ansvarligansattid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_prosjekt_bruker1");

            entity.HasOne(d => d.Kunde).WithMany(p => p.BProsjekts)
                .HasForeignKey(d => d.Kundeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_prosjekt_kunde");
        });

        modelBuilder.Entity<BProsjektUtstyr>(entity =>
        {
            entity.HasKey(e => e.Prosjektid).HasName("b_prosjekt_utstyr_pkey");

            entity.ToTable("b_prosjekt_utstyr");

            entity.Property(e => e.Prosjektid)
                .ValueGeneratedNever()
                .HasColumnName("prosjektid");
            entity.Property(e => e.Datokalibrert).HasColumnName("datokalibrert");
            entity.Property(e => e.Datoregistrert)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.Grabbid).HasColumnName("grabbid");
            entity.Property(e => e.Phehmeter).HasColumnName("phehmeter");
            entity.Property(e => e.Silid).HasColumnName("silid");

            entity.HasOne(d => d.Prosjekt).WithOne(p => p.BProsjektUtstyr)
                .HasForeignKey<BProsjektUtstyr>(d => d.Prosjektid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_prosjekt_utstyr_b_prosjekt");
        });

        modelBuilder.Entity<BProvetakingsplan>(entity =>
        {
            entity.HasKey(e => e.Prosjektid).HasName("b_provetakingsplan_pkey");

            entity.ToTable("b_provetakingsplan");

            entity.Property(e => e.Prosjektid)
                .ValueGeneratedNever()
                .HasColumnName("prosjektid");
            entity.Property(e => e.Datoregistrert)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.Planlagtanalyser)
                .HasMaxLength(45)
                .HasDefaultValueSql("'Parameter I, II og III'::character varying")
                .HasColumnName("planlagtanalyser");
            entity.Property(e => e.Planlagtdybde).HasColumnName("planlagtdybde");
            entity.Property(e => e.Planlagtfeltdato).HasColumnName("planlagtfeltdato");
            entity.Property(e => e.Planlagtkordinatern).HasColumnName("planlagtkordinatern");
            entity.Property(e => e.Planlagtkordinatero).HasColumnName("planlagtkordinatero");
            entity.Property(e => e.Planlegger2id).HasColumnName("planlegger2id");
            entity.Property(e => e.Planleggerid).HasColumnName("planleggerid");
            entity.Property(e => e.Stasjonsid).HasColumnName("stasjonsid");

            entity.HasOne(d => d.Planlegger2).WithMany(p => p.BProvetakingsplanPlanlegger2s)
                .HasForeignKey(d => d.Planlegger2id)
                .HasConstraintName("fk_b_provetakingsplan_bruker2");

            entity.HasOne(d => d.Planlegger).WithMany(p => p.BProvetakingsplanPlanleggers)
                .HasForeignKey(d => d.Planleggerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_provetakingsplan_bruker1");

            entity.HasOne(d => d.Prosjekt).WithOne(p => p.BProvetakingsplan)
                .HasForeignKey<BProvetakingsplan>(d => d.Prosjektid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_provetakingsplan_b_prosjekt");
        });

        modelBuilder.Entity<BRapport>(entity =>
        {
            entity.HasKey(e => e.Rapportid).HasName("b_rapport_pkey");

            entity.ToTable("b_rapport");

            entity.Property(e => e.Rapportid)
                .ValueGeneratedNever()
                .HasColumnName("rapportid");
            entity.Property(e => e.Datoregistrert)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.Generertavid).HasColumnName("generertavid");
            entity.Property(e => e.Godkjentavid).HasColumnName("godkjentavid");
            entity.Property(e => e.Prosjektid).HasColumnName("prosjektid");
            entity.Property(e => e.Rapporttype).HasColumnName("rapporttype");

            entity.HasOne(d => d.Generertav).WithMany(p => p.BRapportGenerertavs)
                .HasForeignKey(d => d.Generertavid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_rapport_ansattgenerert");

            entity.HasOne(d => d.Godkjentav).WithMany(p => p.BRapportGodkjentavs)
                .HasForeignKey(d => d.Godkjentavid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_rapport_ansattgodkjent");

            entity.HasOne(d => d.Prosjekt).WithMany(p => p.BRapports)
                .HasForeignKey(d => d.Prosjektid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_rapport_b_prosjekt");
        });

        modelBuilder.Entity<BSensorisk>(entity =>
        {
            entity.HasKey(e => new { e.Prosjektid, e.Stasjonsid }).HasName("b_sensorisk_pkey");

            entity.ToTable("b_sensorisk");

            entity.Property(e => e.Prosjektid).HasColumnName("prosjektid");
            entity.Property(e => e.Stasjonsid).HasColumnName("stasjonsid");
            entity.Property(e => e.Farge).HasColumnName("farge");
            entity.Property(e => e.Gassbobler).HasColumnName("gassbobler");
            entity.Property(e => e.Grabbvolum).HasColumnName("grabbvolum");
            entity.Property(e => e.Konsistens).HasColumnName("konsistens");
            entity.Property(e => e.Lukt).HasColumnName("lukt");
            entity.Property(e => e.ProveEh).HasColumnName("prove_eh");
            entity.Property(e => e.ProvePh).HasColumnName("prove_ph");
            entity.Property(e => e.ProveTemperatur).HasColumnName("prove_temperatur");
            entity.Property(e => e.Tykkelseslamlag).HasColumnName("tykkelseslamlag");

            entity.HasOne(d => d.FargeNavigation).WithMany(p => p.BSensorisks)
                .HasPrincipalKey(p => p.Verdi)
                .HasForeignKey(d => d.Farge)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_sensorisk_sys_farge");

            entity.HasOne(d => d.GassboblerNavigation).WithMany(p => p.BSensorisks)
                .HasForeignKey(d => d.Gassbobler)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_sensorisk_sys_gassbobler");

            entity.HasOne(d => d.GrabbvolumNavigation).WithMany(p => p.BSensorisks)
                .HasPrincipalKey(p => p.Verdi)
                .HasForeignKey(d => d.Grabbvolum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_sensorisk_sys_grabbvolum");

            entity.HasOne(d => d.KonsistensNavigation).WithMany(p => p.BSensorisks)
                .HasPrincipalKey(p => p.Verdi)
                .HasForeignKey(d => d.Konsistens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_sensorisk_sys_konsistens");

            entity.HasOne(d => d.LuktNavigation).WithMany(p => p.BSensorisks)
                .HasPrincipalKey(p => p.Verdi)
                .HasForeignKey(d => d.Lukt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_sensorisk_sys_lukt");

            entity.HasOne(d => d.TykkelseslamlagNavigation).WithMany(p => p.BSensorisks)
                .HasPrincipalKey(p => p.Verdi)
                .HasForeignKey(d => d.Tykkelseslamlag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_sensorisk_sys_tykkelsepaslam");

            entity.HasOne(d => d.BStasjon).WithOne(p => p.BSensorisk)
                .HasForeignKey<BSensorisk>(d => new { d.Prosjektid, d.Stasjonsid })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_sensorisk_b_stasjon");
        });

        modelBuilder.Entity<BStasjon>(entity =>
        {
            entity.HasKey(e => new { e.Prosjektid, e.Stasjonsid }).HasName("b_stasjon_pkey");

            entity.ToTable("b_stasjon");

            entity.Property(e => e.Prosjektid).HasColumnName("prosjektid");
            entity.Property(e => e.Stasjonsid).HasColumnName("stasjonsid");
            entity.Property(e => e.Nummer).HasColumnName("nummer");
            entity.Property(e => e.Antallgrabbskudd).HasColumnName("antallgrabbskudd");
            entity.Property(e => e.Arter)
                .HasMaxLength(225)
                .HasColumnName("arter");
            entity.Property(e => e.Bunnsammensettningid).HasColumnName("bunnsammensettningid");
            entity.Property(e => e.Bunntype).HasColumnName("bunntype");
            entity.Property(e => e.Datokalibrert).HasColumnName("datokalibrert");
            entity.Property(e => e.Datoregistrert)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.Dybde).HasColumnName("dybde");
            entity.Property(e => e.Dyr).HasColumnName("dyr");
            entity.Property(e => e.Grabbid).HasColumnName("grabbid");
            entity.Property(e => e.Grabhastighetgodkjent).HasColumnName("grabhastighetgodkjent");
            entity.Property(e => e.Kordinatern).HasColumnName("kordinatern");
            entity.Property(e => e.Kordinatero).HasColumnName("kordinatero");
            entity.Property(e => e.Korrigering)
                .HasMaxLength(225)
                .HasColumnName("korrigering");
            entity.Property(e => e.Merknad)
                .HasMaxLength(225)
                .HasColumnName("merknad");
            entity.Property(e => e.Phehmeter).HasColumnName("phehmeter");
            entity.Property(e => e.Sensoriskutfort).HasColumnName("sensoriskutfort");
            entity.Property(e => e.Silid).HasColumnName("silid");
            entity.Property(e => e.SkjovannEh).HasColumnName("skjovann_eh");
            entity.Property(e => e.SkjovannPh).HasColumnName("skjovann_ph");
            entity.Property(e => e.SkjovannTemperatur).HasColumnName("skjovann_temperatur");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");

            entity.HasOne(d => d.Bunnsammensettning).WithMany(p => p.BStasjons)
                .HasForeignKey(d => d.Bunnsammensettningid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_stasjon_sys_bunsammensettning");

            entity.HasOne(d => d.Prosjekt).WithMany(p => p.BStasjons)
                .HasForeignKey(d => d.Prosjektid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_stasjon_b_prosjekt");
        });

        modelBuilder.Entity<Endringslogg>(entity =>
        {
            entity.HasKey(e => e.Loggid).HasName("endringslogg_pkey");

            entity.ToTable("endringslogg");

            entity.Property(e => e.Loggid).HasColumnName("loggid");
            entity.Property(e => e.Datoregistrert)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.Endretavid).HasColumnName("endretavid");
            entity.Property(e => e.Prosjektid).HasColumnName("prosjektid");
            entity.Property(e => e.Stasjonsid).HasColumnName("stasjonsid");
            entity.Property(e => e.Tabellendret)
                .HasMaxLength(45)
                .HasColumnName("tabellendret");
            entity.Property(e => e.Typeending)
                .HasMaxLength(45)
                .HasColumnName("typeending");
            entity.Property(e => e.Verdiendret)
                .HasMaxLength(45)
                .HasColumnName("verdiendret");
            entity.Property(e => e.Verdiendretfra)
                .HasMaxLength(45)
                .HasColumnName("verdiendretfra");
            entity.Property(e => e.Verdiendrettil)
                .HasMaxLength(45)
                .HasColumnName("verdiendrettil");

            entity.HasOne(d => d.Endretav).WithMany(p => p.Endringsloggs)
                .HasForeignKey(d => d.Endretavid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_endringslogg_bruker");
        });

        modelBuilder.Entity<Kunde>(entity =>
        {
            entity.HasKey(e => e.Kundeid).HasName("kunde_pkey");

            entity.ToTable("kunde");

            entity.Property(e => e.Kundeid).HasColumnName("kundeid");
            entity.Property(e => e.Oppdragsgiver)
                .HasMaxLength(45)
                .HasColumnName("oppdragsgiver");
            entity.Property(e => e.Kontaktperson)
                .HasMaxLength(45)
                .HasColumnName("kontaktperson");
            entity.Property(e => e.Telefonnummer)
                .HasMaxLength(45)
                .HasColumnName("telefonnummer");
        });

        modelBuilder.Entity<Revisjonslogg>(entity =>
        {
            entity.HasKey(e => e.Revisjonsid).HasName("revisjonslogg_pkey");

            entity.ToTable("revisjonslogg");

            entity.Property(e => e.Revisjonsid)
                .ValueGeneratedNever()
                .HasColumnName("revisjonsid");
            entity.Property(e => e.Datoregistrert)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.Gjeldenderevisjon).HasColumnName("gjeldenderevisjon");
            entity.Property(e => e.Revisjonskommentar)
                .HasMaxLength(255)
                .HasColumnName("revisjonskommentar");
        });

        modelBuilder.Entity<SysArter>(entity =>
        {
            entity.HasKey(e => e.Artid).HasName("sys_arter_pkey");

            entity.ToTable("sys_arter");

            entity.Property(e => e.Artid).HasColumnName("artid");
            entity.Property(e => e.Artsforkortelse)
                .HasMaxLength(45)
                .HasColumnName("artsforkortelse");
            entity.Property(e => e.Artsnavn)
                .HasMaxLength(45)
                .HasColumnName("artsnavn");
        });

        modelBuilder.Entity<SysBunsammensettning>(entity =>
        {
            entity.HasKey(e => e.Bunnsammensettningid).HasName("sys_bunsammensettning_pkey");

            entity.ToTable("sys_bunsammensettning");

            entity.Property(e => e.Bunnsammensettningid).HasColumnName("bunnsammensettningid");
            entity.Property(e => e.Fjellbunn).HasColumnName("fjellbunn");
            entity.Property(e => e.Grus).HasColumnName("grus");
            entity.Property(e => e.Leire).HasColumnName("leire");
            entity.Property(e => e.Sand).HasColumnName("sand");
            entity.Property(e => e.Silt).HasColumnName("silt");
            entity.Property(e => e.Skjellsand).HasColumnName("skjellsand");
            entity.Property(e => e.Steinbunn).HasColumnName("steinbunn");
        });

        modelBuilder.Entity<SysFarge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_farge_pkey");

            entity.ToTable("sys_farge");

            entity.HasIndex(e => e.Verdi, "sys_farge_verdi_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Beskrivelse)
                .HasMaxLength(45)
                .HasColumnName("beskrivelse");
            entity.Property(e => e.Verdi).HasColumnName("verdi");
        });

        modelBuilder.Entity<SysGassbobler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_gassbobler_pkey");

            entity.ToTable("sys_gassbobler");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Beskrivelse)
                .HasMaxLength(45)
                .HasColumnName("beskrivelse");
            entity.Property(e => e.Verdi).HasColumnName("verdi");
        });

        modelBuilder.Entity<SysGrabbvolum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_grabbvolum_pkey");

            entity.ToTable("sys_grabbvolum");

            entity.HasIndex(e => e.Verdi, "sys_grabbvolum_verdi_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Beskrivelse)
                .HasMaxLength(45)
                .HasColumnName("beskrivelse");
            entity.Property(e => e.Verdi).HasColumnName("verdi");
        });

        modelBuilder.Entity<SysKonsisten>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_konsistens_pkey");

            entity.ToTable("sys_konsistens");

            entity.HasIndex(e => e.Verdi, "sys_konsistens_verdi_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Beskrivelse)
                .HasMaxLength(45)
                .HasColumnName("beskrivelse");
            entity.Property(e => e.Verdi).HasColumnName("verdi");
        });

        modelBuilder.Entity<SysLukt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_lukt_pkey");

            entity.ToTable("sys_lukt");

            entity.HasIndex(e => e.Verdi, "sys_lukt_verdi_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Beskrivelse)
                .HasMaxLength(45)
                .HasColumnName("beskrivelse");
            entity.Property(e => e.Verdi).HasColumnName("verdi");
        });

        modelBuilder.Entity<SysMerknad>(entity =>
        {
            entity.HasKey(e => e.Merknadid).HasName("sys_merknad_pkey");

            entity.ToTable("sys_merknad");

            entity.Property(e => e.Merknadid)
                .ValueGeneratedNever()
                .HasColumnName("merknadid");
            entity.Property(e => e.Beskrivelse)
                .HasMaxLength(255)
                .HasColumnName("beskrivelse");
            entity.Property(e => e.Forkortelse)
                .HasMaxLength(45)
                .HasColumnName("forkortelse");
        });

        modelBuilder.Entity<SysTykkelsepaslam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_tykkelsepaslam_pkey");

            entity.ToTable("sys_tykkelsepaslam");

            entity.HasIndex(e => e.Verdi, "sys_tykkelsepaslam_verdi_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Beskrivelse)
                .HasMaxLength(45)
                .HasColumnName("beskrivelse");
            entity.Property(e => e.Verdi).HasColumnName("verdi");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_pkey");

            entity.ToTable("token");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiredAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expired_at");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.Token1)
                .HasColumnType("character varying")
                .HasColumnName("token");
            entity.Property(e => e.UsedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("used_at");
            entity.Property(e => e.BrukerId).HasColumnName("bruker_id");

            entity.HasOne(d => d.Bruker).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.BrukerId)
                .HasConstraintName("fk_token_bruker");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
