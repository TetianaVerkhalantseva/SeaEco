using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BSediment
{
    public Guid Id { get; set; }

    public float Ph { get; set; }

    public float Eh { get; set; }

    public float Temperatur { get; set; }

    public int? KlasseGr2 { get; set; }

    public int? TilstandGr2 { get; set; }

    public virtual BUndersokelse? BUndersokelse { get; set; }
}
