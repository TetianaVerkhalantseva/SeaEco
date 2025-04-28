using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BPreinfo
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public DateTime Feltdato { get; set; }

    public Guid FeltansvarligId { get; set; }

    public virtual BSjovann? BSjovann { get; set; }

    public virtual ICollection<BUndersokelse> BUndersokelses { get; set; } = new List<BUndersokelse>();

    public virtual BUtstyrsid? BUtstyrsid { get; set; }

    public virtual Bruker Feltansvarlig { get; set; } = null!;

    public virtual BProsjekt Prosjekt { get; set; } = null!;

    public virtual ICollection<Bruker> Provetakers { get; set; } = new List<Bruker>();
}
