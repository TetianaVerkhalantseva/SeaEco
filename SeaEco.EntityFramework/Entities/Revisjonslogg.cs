using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class Revisjonslogg
{
    public int Revisjonsid { get; set; }

    public string Revisjonskommentar { get; set; } = null!;

    public bool Gjeldenderevisjon { get; set; }

    public DateTime? Datoregistrert { get; set; }
}
