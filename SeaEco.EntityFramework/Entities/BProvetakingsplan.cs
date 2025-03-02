using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BProvetakingsplan
{
    public int Prosjektid { get; set; }

    public int Planleggerid { get; set; }

    public int? Planlegger2id { get; set; }

    public int Stasjonsid { get; set; }

    public DateOnly Planlagtfeltdato { get; set; }

    public int Planlagtdybde { get; set; }

    public int Planlagtkordinatern { get; set; }

    public int Planlagtkordinatero { get; set; }

    public string Planlagtanalyser { get; set; } = null!;

    public DateTime Datoregistrert { get; set; }

    public virtual Ansatte Planlegger { get; set; } = null!;

    public virtual Ansatte? Planlegger2 { get; set; }

    public virtual BProsjekt Prosjekt { get; set; } = null!;
}
