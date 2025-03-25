using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BProvetakingsplan
{
    public Guid Prosjektid { get; set; }

    public Guid Planleggerid { get; set; }

    public Guid? Planlegger2id { get; set; }

    public Guid Stasjonsid { get; set; }

    public DateOnly Planlagtfeltdato { get; set; }

    public int Planlagtdybde { get; set; }

    public int Planlagtkordinatern { get; set; }

    public int Planlagtkordinatero { get; set; }

    public string Planlagtanalyser { get; set; } = null!;

    public DateTime Datoregistrert { get; set; }

    public virtual Bruker Planlegger { get; set; } = null!;

    public virtual Bruker? Planlegger2 { get; set; }

    public virtual BProsjekt Prosjekt { get; set; } = null!;
}
