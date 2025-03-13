using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class Endringslogg
{
    public int Loggid { get; set; }

    public int Prosjektid { get; set; }

    public int? Stasjonsid { get; set; }

    public string Tabellendret { get; set; } = null!;

    public string Typeending { get; set; } = null!;

    public string Verdiendret { get; set; } = null!;

    public string Verdiendretfra { get; set; } = null!;

    public string Verdiendrettil { get; set; } = null!;

    public Guid Endretavid { get; set; }

    public DateTime Datoregistrert { get; set; }

    public virtual Bruker Endretav { get; set; } = null!;
}
