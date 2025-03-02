using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BRapport
{
    public int Rapportid { get; set; }

    public int Prosjektid { get; set; }

    public int Rapporttype { get; set; }

    public DateTime? Datoregistrert { get; set; }

    public int Generertavid { get; set; }

    public int Godkjentavid { get; set; }

    public virtual Ansatte Generertav { get; set; } = null!;

    public virtual Ansatte Godkjentav { get; set; } = null!;

    public virtual BProsjekt Prosjekt { get; set; } = null!;
}
