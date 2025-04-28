using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BProsjekt
{
    public Guid Id { get; set; }

    public string? ProsjektIdSe { get; set; }

    public string PoId { get; set; } = null!;

    public Guid KundeId { get; set; }

    public string Kundekontaktperson { get; set; } = null!;

    public string Kundetlf { get; set; } = null!;

    public string? Kundeepost { get; set; }

    public Guid LokalitetId { get; set; }

    public int? Mtbtillatelse { get; set; }

    public Guid ProsjektansvarligId { get; set; }

    public int Produksjonsstatus { get; set; }

    public string? Merknad { get; set; }

    public int Prosjektstatus { get; set; }

    public DateTime Datoregistrert { get; set; }

    public virtual ICollection<BPreinfo> BPreinfos { get; set; } = new List<BPreinfo>();

    public virtual BProvetakningsplan? BProvetakningsplan { get; set; }

    public virtual ICollection<BStasjon> BStasjons { get; set; } = new List<BStasjon>();

    public virtual BTilstand? BTilstand { get; set; }

    public virtual ICollection<BUndersokelse> BUndersokelses { get; set; } = new List<BUndersokelse>();

    public virtual Kunde Kunde { get; set; } = null!;

    public virtual Lokalitet Lokalitet { get; set; } = null!;

    public virtual Bruker Prosjektansvarlig { get; set; } = null!;
}
