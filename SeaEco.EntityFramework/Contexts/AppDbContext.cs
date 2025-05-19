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
    }

    public virtual DbSet<BBilder> BBilders { get; set; }

    public virtual DbSet<BBlotbunn> BBlotbunns { get; set; }

    public virtual DbSet<BDyr> BDyrs { get; set; }

    public virtual DbSet<BHardbunn> BHardbunns { get; set; }

    public virtual DbSet<BPreinfo> BPreinfos { get; set; }

    public virtual DbSet<BProsjekt> BProsjekts { get; set; }

    public virtual DbSet<BProvetakningsplan> BProvetakningsplans { get; set; }

    public virtual DbSet<BRapporter> BRapporters { get; set; }

    public virtual DbSet<BSediment> BSediments { get; set; }

    public virtual DbSet<BSensorisk> BSensorisks { get; set; }

    public virtual DbSet<BStasjon> BStasjons { get; set; }

    public virtual DbSet<BTilstand> BTilstands { get; set; }

    public virtual DbSet<BUndersokelse> BUndersokelses { get; set; }

    public virtual DbSet<BUndersokelseslogg> BUndersokelsesloggs { get; set; }

    public virtual DbSet<Bruker> Brukers { get; set; }

    public virtual DbSet<Kunde> Kundes { get; set; }

    public virtual DbSet<Lokalitet> Lokalitets { get; set; }

    public virtual DbSet<Programversjon> Programversjons { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=127.0.0.1;Host=localhost;Database=seaeco;Port=5432;username=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BBilder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_bilder");

            entity.ToTable("b_bilder");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Datogenerert)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datogenerert");
            entity.Property(e => e.Extension).HasColumnName("extension");
            entity.Property(e => e.Silt).HasColumnName("silt");
            entity.Property(e => e.UndersokelseId).HasColumnName("undersokelse_id");

            entity.HasOne(d => d.Undersokelse).WithMany(p => p.BBilders)
                .HasForeignKey(d => d.UndersokelseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bbilder_undersokelse_id");
        });

        modelBuilder.Entity<BBlotbunn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_blotbunn");

            entity.ToTable("b_blotbunn");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Grus).HasColumnName("grus");
            entity.Property(e => e.Leire).HasColumnName("leire");
            entity.Property(e => e.Sand).HasColumnName("sand");
            entity.Property(e => e.Silt).HasColumnName("silt");
            entity.Property(e => e.Skjellsand).HasColumnName("skjellsand");
        });

        modelBuilder.Entity<BDyr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_dyr");

            entity.ToTable("b_dyr");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Arter).HasColumnName("arter");
            entity.Property(e => e.Borstemark).HasColumnName("borstemark");
            entity.Property(e => e.Krepsdyr).HasColumnName("krepsdyr");
            entity.Property(e => e.Pigghunder).HasColumnName("pigghunder");
            entity.Property(e => e.Skjell).HasColumnName("skjell");
        });

        modelBuilder.Entity<BHardbunn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_hardbunn");

            entity.ToTable("b_hardbunn");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Fjellbunn).HasColumnName("fjellbunn");
            entity.Property(e => e.Steinbunn).HasColumnName("steinbunn");
        });

        modelBuilder.Entity<BPreinfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_preinfo");

            entity.ToTable("b_preinfo");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.EhSjo).HasColumnName("eh_sjo");
            entity.Property(e => e.FeltansvarligId).HasColumnName("feltansvarlig_id");
            entity.Property(e => e.Feltdato)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("feltdato");
            entity.Property(e => e.Grabb).HasColumnName("grabb");
            entity.Property(e => e.Kalibreringsdato).HasColumnName("kalibreringsdato");
            entity.Property(e => e.PhMeter).HasColumnName("ph_meter");
            entity.Property(e => e.PhSjo).HasColumnName("ph_sjo");
            entity.Property(e => e.ProsjektId).HasColumnName("prosjekt_id");
            entity.Property(e => e.RefElektrode)
                .HasDefaultValue(0)
                .HasColumnName("ref_elektrode");
            entity.Property(e => e.Sil).HasColumnName("sil");
            entity.Property(e => e.SjoTemperatur).HasColumnName("sjo_temperatur");

            entity.HasOne(d => d.Feltansvarlig).WithMany(p => p.BPreinfos)
                .HasForeignKey(d => d.FeltansvarligId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bpreinfo_bruker_id");

            entity.HasOne(d => d.Prosjekt).WithMany(p => p.BPreinfos)
                .HasForeignKey(d => d.ProsjektId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bpreinfo_prosjekt_id");
        });

        modelBuilder.Entity<BProsjekt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_prosjekt");

            entity.ToTable("b_prosjekt");

            entity.HasIndex(e => e.PoId, "uq_bprosjekt_poID").IsUnique();

            entity.HasIndex(e => e.ProsjektIdSe, "uq_bprosjekt_prosjektIdSe").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Datoregistrert)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datoregistrert");
            entity.Property(e => e.KundeId).HasColumnName("kunde_id");
            entity.Property(e => e.Kundeepost)
                .HasMaxLength(45)
                .HasColumnName("kundeepost");
            entity.Property(e => e.Kundekontaktperson)
                .HasMaxLength(45)
                .HasColumnName("kundekontaktperson");
            entity.Property(e => e.Kundetlf)
                .HasMaxLength(12)
                .HasColumnName("kundetlf");
            entity.Property(e => e.LokalitetId).HasColumnName("lokalitet_id");
            entity.Property(e => e.Merknad).HasColumnName("merknad");
            entity.Property(e => e.Mtbtillatelse).HasColumnName("mtbtillatelse");
            entity.Property(e => e.PoId)
                .HasMaxLength(6)
                .HasColumnName("poID");
            entity.Property(e => e.Produksjonsstatus).HasColumnName("produksjonsstatus");
            entity.Property(e => e.ProsjektIdSe)
                .HasMaxLength(12)
                .HasColumnName("prosjektIdSe");
            entity.Property(e => e.ProsjektansvarligId).HasColumnName("prosjektansvarlig_id");
            entity.Property(e => e.Prosjektstatus).HasColumnName("prosjektstatus");

            entity.HasOne(d => d.Kunde).WithMany(p => p.BProsjekts)
                .HasForeignKey(d => d.KundeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bprosjekt_kunde_id");

            entity.HasOne(d => d.Lokalitet).WithMany(p => p.BProsjekts)
                .HasForeignKey(d => d.LokalitetId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bprosjekt_lokalitet_id");

            entity.HasOne(d => d.Prosjektansvarlig).WithMany(p => p.BProsjekts)
                .HasForeignKey(d => d.ProsjektansvarligId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bprosjekt_bruker_id");
        });

        modelBuilder.Entity<BProvetakningsplan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_provetakningsplan");

            entity.ToTable("b_provetakningsplan");

            entity.HasIndex(e => e.ProsjektId, "uq_bprovetakningsplan_prosjekt_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Planlagtfeltdato).HasColumnName("planlagtfeltdato");
            entity.Property(e => e.PlanleggerId).HasColumnName("planlegger_id");
            entity.Property(e => e.ProsjektId).HasColumnName("prosjekt_id");

            entity.HasOne(d => d.Planlegger).WithMany(p => p.BProvetakningsplans)
                .HasForeignKey(d => d.PlanleggerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bprovetakningsplan_bruker_id");

            entity.HasOne(d => d.Prosjekt).WithOne(p => p.BProvetakningsplan)
                .HasForeignKey<BProvetakningsplan>(d => d.ProsjektId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bprovetakingsplan_prosjekt_id");
        });

        modelBuilder.Entity<BRapporter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_rapporter");

            entity.ToTable("b_rapporter");

            entity.HasIndex(e => e.GodkjentAv, "fki_fk_brapporter_bruker_id");

            entity.HasIndex(e => e.ProsjektId, "fki_fk_brapporter_prosjekt_id");

            entity.HasIndex(e => new { e.ProsjektId, e.ArkNavn }, "uq_brapporter_prosjektid_arknavn").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ArkNavn).HasColumnName("ark_navn");
            entity.Property(e => e.Datogenerert)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datogenerert");
            entity.Property(e => e.ErGodkjent)
                .HasDefaultValue(false)
                .HasColumnName("er_godkjent");
            entity.Property(e => e.GodkjentAv).HasColumnName("godkjent_av");
            entity.Property(e => e.ProsjektId).HasColumnName("prosjekt_id");

            entity.HasOne(d => d.GodkjentAvNavigation).WithMany(p => p.BRapporters)
                .HasForeignKey(d => d.GodkjentAv)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_brapporter_bruker_id");

            entity.HasOne(d => d.Prosjekt).WithMany(p => p.BRapporters)
                .HasForeignKey(d => d.ProsjektId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_brapporter_prosjekt_id");
        });

        modelBuilder.Entity<BSediment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_sediment");

            entity.ToTable("b_sediment");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Eh).HasColumnName("eh");
            entity.Property(e => e.KlasseGr2).HasColumnName("klasse_gr2");
            entity.Property(e => e.Ph).HasColumnName("ph");
            entity.Property(e => e.Temperatur).HasColumnName("temperatur");
            entity.Property(e => e.TilstandGr2).HasColumnName("tilstand_gr2");
        });

        modelBuilder.Entity<BSensorisk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_sensorisk");

            entity.ToTable("b_sensorisk");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Farge).HasColumnName("farge");
            entity.Property(e => e.Gassbobler).HasColumnName("gassbobler");
            entity.Property(e => e.Grabbvolum).HasColumnName("grabbvolum");
            entity.Property(e => e.IndeksGr3).HasColumnName("indeks_gr3");
            entity.Property(e => e.Konsistens).HasColumnName("konsistens");
            entity.Property(e => e.Lukt).HasColumnName("lukt");
            entity.Property(e => e.TilstandGr3).HasColumnName("tilstand _gr3");
            entity.Property(e => e.Tykkelseslamlag).HasColumnName("tykkelseslamlag");
        });

        modelBuilder.Entity<BStasjon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_stasjon");

            entity.ToTable("b_stasjon");

            entity.HasIndex(e => new { e.ProsjektId, e.Nummer }, "uq_bstasjon_prosjektid_nummer").IsUnique();

            entity.HasIndex(e => e.UndersokelseId, "uq_bstasjon_undersokelse_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Analyser).HasColumnName("analyser");
            entity.Property(e => e.Dybde).HasColumnName("dybde");
            entity.Property(e => e.KoordinatNord).HasColumnName("koordinat_nord");
            entity.Property(e => e.KoordinatOst).HasColumnName("koordinat_ost");
            entity.Property(e => e.Nummer).HasColumnName("nummer");
            entity.Property(e => e.ProsjektId).HasColumnName("prosjekt_id");
            entity.Property(e => e.ProvetakingsplanId).HasColumnName("provetakingsplan_id");
            entity.Property(e => e.UndersokelseId).HasColumnName("undersokelse_id");

            entity.HasOne(d => d.Prosjekt).WithMany(p => p.BStasjons)
                .HasForeignKey(d => d.ProsjektId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bstasjon_prosjekt_id ");

            entity.HasOne(d => d.Provetakingsplan).WithMany(p => p.BStasjons)
                .HasForeignKey(d => d.ProvetakingsplanId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bstasjon_provetakingsplan_id ");

            entity.HasOne(d => d.Undersokelse).WithOne(p => p.BStasjon)
                .HasForeignKey<BStasjon>(d => d.UndersokelseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bstasjon_undersokelse_id ");
        });

        modelBuilder.Entity<BTilstand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_tilstand");

            entity.ToTable("b_tilstand");

            entity.HasIndex(e => e.ProsjektId, "uq_btilstand_prosjekt_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IndeksGr2).HasColumnName("indeks_gr2");
            entity.Property(e => e.IndeksGr3).HasColumnName("indeks_gr3");
            entity.Property(e => e.IndeksLokalitet).HasColumnName("indeks_lokalitet");
            entity.Property(e => e.ProsjektId).HasColumnName("prosjekt_id");
            entity.Property(e => e.TilstandGr2).HasColumnName("tilstand_gr2");
            entity.Property(e => e.TilstandGr3).HasColumnName("tilstand_gr3");
            entity.Property(e => e.TilstandLokalitet).HasColumnName("tilstand_lokalitet");

            entity.HasOne(d => d.Prosjekt).WithOne(p => p.BTilstand)
                .HasForeignKey<BTilstand>(d => d.ProsjektId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_btilstand_prosjekt_id");
        });

        modelBuilder.Entity<BUndersokelse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_undersokelse");

            entity.ToTable("b_undersokelse");

            entity.HasIndex(e => e.BlotbunnId, "uq_bundersokelse_blotbunn_id").IsUnique();

            entity.HasIndex(e => e.DyrId, "uq_bundersokelse_dyr_id").IsUnique();

            entity.HasIndex(e => e.HardbunnId, "uq_bundersokelse_hardbunn_id").IsUnique();

            entity.HasIndex(e => e.SedimentId, "uq_bundersokelse_sediment_id").IsUnique();

            entity.HasIndex(e => e.SensoriskId, "uq_bundersokelse_sensorisk_id").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AntallGrabbhugg).HasColumnName("antall_grabbhugg");
            entity.Property(e => e.Beggiatoa).HasColumnName("beggiatoa");
            entity.Property(e => e.BlotbunnId).HasColumnName("blotbunn_id");
            entity.Property(e => e.DatoEndret)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dato_endret");
            entity.Property(e => e.DatoRegistrert)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dato_registrert");
            entity.Property(e => e.DyrId).HasColumnName("dyr_id");
            entity.Property(e => e.Fekalier).HasColumnName("fekalier");
            entity.Property(e => e.Feltdato).HasColumnName("feltdato");
            entity.Property(e => e.Forrester).HasColumnName("forrester");
            entity.Property(e => e.GrabbhastighetGodkjent).HasColumnName("grabbhastighet_godkjent");
            entity.Property(e => e.HardbunnId).HasColumnName("hardbunn_id");
            entity.Property(e => e.IndeksGr2Gr3).HasColumnName("indeks_gr2_gr3");
            entity.Property(e => e.Merknader).HasColumnName("merknader");
            entity.Property(e => e.PreinfoId).HasColumnName("preinfo_id");
            entity.Property(e => e.ProsjektId).HasColumnName("prosjekt_id");
            entity.Property(e => e.SedimentId).HasColumnName("sediment_id");
            entity.Property(e => e.SensoriskId).HasColumnName("sensorisk_id");
            entity.Property(e => e.TilstandGr2Gr3).HasColumnName("tilstand_gr2_gr3");

            entity.HasOne(d => d.Blotbunn).WithOne(p => p.BUndersokelse)
                .HasForeignKey<BUndersokelse>(d => d.BlotbunnId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bundersokelse_blotbunn_id");

            entity.HasOne(d => d.Dyr).WithOne(p => p.BUndersokelse)
                .HasForeignKey<BUndersokelse>(d => d.DyrId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bundersokelse_dyr_id");

            entity.HasOne(d => d.Hardbunn).WithOne(p => p.BUndersokelse)
                .HasForeignKey<BUndersokelse>(d => d.HardbunnId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bundersøkelse_hardbunn_id");

            entity.HasOne(d => d.Preinfo).WithMany(p => p.BUndersokelses)
                .HasForeignKey(d => d.PreinfoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bundersokelse_preinfo_id");

            entity.HasOne(d => d.Prosjekt).WithMany(p => p.BUndersokelses)
                .HasForeignKey(d => d.ProsjektId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bundersokelse_prosjekt_id");

            entity.HasOne(d => d.Sediment).WithOne(p => p.BUndersokelse)
                .HasForeignKey<BUndersokelse>(d => d.SedimentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bundersokelse_sediment_id");

            entity.HasOne(d => d.Sensorisk).WithOne(p => p.BUndersokelse)
                .HasForeignKey<BUndersokelse>(d => d.SensoriskId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bundersokelse_sensorisk_id");
        });

        modelBuilder.Entity<BUndersokelseslogg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_b_undersokelseslogg");

            entity.ToTable("b_undersokelseslogg");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DatoEndret)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dato_endret");
            entity.Property(e => e.EndretAv).HasColumnName("endret_av");
            entity.Property(e => e.Felt).HasColumnName("felt");
            entity.Property(e => e.GammelVerdi).HasColumnName("gammel_verdi");
            entity.Property(e => e.NyVerdi).HasColumnName("ny_verdi");
            entity.Property(e => e.UndersokelseId).HasColumnName("undersokelse_id");

            entity.HasOne(d => d.EndretAvNavigation).WithMany(p => p.BUndersokelsesloggs)
                .HasForeignKey(d => d.EndretAv)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bundersokelseslogg_bruker_id");

            entity.HasOne(d => d.Undersokelse).WithMany(p => p.BUndersokelsesloggs)
                .HasForeignKey(d => d.UndersokelseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_bundersokelseslogg_undersokelse_id");
        });

        modelBuilder.Entity<Bruker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_bruker");

            entity.ToTable("bruker");

            entity.HasIndex(e => e.Epost, "uq_bruker_epost").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Aktiv).HasColumnName("aktiv");
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
            entity.Property(e => e.Salt).HasColumnName("salt");

            entity.HasMany(d => d.Preinfos).WithMany(p => p.Provetakers)
                .UsingEntity<Dictionary<string, object>>(
                    "BpreinfoProvetaker",
                    r => r.HasOne<BPreinfo>().WithMany()
                        .HasForeignKey("PreinfoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_bpreinfo_provetaker_preinfo_id"),
                    l => l.HasOne<Bruker>().WithMany()
                        .HasForeignKey("ProvetakerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_bpreinfo_provetaker_bruker_id"),
                    j =>
                    {
                        j.HasKey("ProvetakerId", "PreinfoId").HasName("pk_provetaker_bpreinfo");
                        j.ToTable("bpreinfo_provetaker");
                        j.IndexerProperty<Guid>("ProvetakerId").HasColumnName("provetaker_id");
                        j.IndexerProperty<Guid>("PreinfoId").HasColumnName("preinfo_id");
                    });
        });

        modelBuilder.Entity<Kunde>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_kunde");

            entity.ToTable("kunde");

            entity.HasIndex(e => e.Oppdragsgiver, "uq_kunde_oppdragsgiver").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Kontaktperson)
                .HasMaxLength(45)
                .HasColumnName("kontaktperson");
            entity.Property(e => e.Oppdragsgiver)
                .HasMaxLength(45)
                .HasColumnName("oppdragsgiver");
            entity.Property(e => e.Telefon)
                .HasMaxLength(20)
                .HasColumnName("telefon");
        });

        modelBuilder.Entity<Lokalitet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_lokalitet");

            entity.ToTable("lokalitet");

            entity.HasIndex(e => e.LokalitetsId, "uq_lokalitet_lokalitetsid").IsUnique();

            entity.HasIndex(e => e.Lokalitetsnavn, "uq_lokalitet_lokalitetsnavn").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LokalitetsId)
                .HasMaxLength(5)
                .HasColumnName("lokalitetsID");
            entity.Property(e => e.Lokalitetsnavn)
                .HasMaxLength(50)
                .HasColumnName("lokalitetsnavn");
        });

        modelBuilder.Entity<Programversjon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_programversjon");

            entity.ToTable("programversjon");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ErAktiv).HasColumnName("er_aktiv");
            entity.Property(e => e.Forbedringer).HasColumnName("forbedringer");
            entity.Property(e => e.Utgivelsesdato).HasColumnName("utgivelsesdato");
            entity.Property(e => e.Versjonsnummer)
                .HasMaxLength(50)
                .HasColumnName("versjonsnummer");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_token");

            entity.ToTable("token");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BrukerId).HasColumnName("bruker_id");
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

            entity.HasOne(d => d.Bruker).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.BrukerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_token_bruker");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
