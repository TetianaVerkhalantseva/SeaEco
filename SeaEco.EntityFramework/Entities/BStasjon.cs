using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BStasjon
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public int Nummer { get; set; }

    public string KoordinatNord { get; set; } = null!;

    public string KoordinatOst { get; set; } = null!;

    public int Dybde { get; set; }

    public string Analyser { get; set; } = null!;

    public Guid? ProvetakingsplanId { get; set; }

    public Guid? UndersokelseId { get; set; }

    public virtual BProsjekt Prosjekt { get; set; } = null!;

    public virtual BProvetakningsplan? Provetakingsplan { get; set; }

    public virtual BUndersokelse? Undersokelse { get; set; }
}
