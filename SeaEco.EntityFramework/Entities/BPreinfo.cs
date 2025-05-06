using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BPreinfo
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public DateTime Feltdato { get; set; }

    public Guid FeltansvarligId { get; set; }

    public float PhSjo { get; set; }

    public float EhSjo { get; set; }

    public float SjoTemperatur { get; set; }

    public int RefElektrode { get; set; }

    public string Grabb { get; set; } = null!;

    public string Sil { get; set; } = null!;

    public string PhMeter { get; set; } = null!;

    public DateOnly Kalibreringsdato { get; set; }

    public virtual ICollection<BUndersokelse> BUndersokelses { get; set; } = new List<BUndersokelse>();

    public virtual Bruker Feltansvarlig { get; set; } = null!;

    public virtual BProsjekt Prosjekt { get; set; } = null!;

    public virtual ICollection<Bruker> Provetakers { get; set; } = new List<Bruker>();
}
