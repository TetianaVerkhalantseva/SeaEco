using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BRapport
{
    public int Rapportid { get; set; }

    public int Prosjektid { get; set; }

    public int Rapporttype { get; set; }

    public DateTime? Datoregistrert { get; set; }

    public Guid Generertavid { get; set; }

    public Guid Godkjentavid { get; set; }

    public virtual User Generertav { get; set; } = null!;

    public virtual User Godkjentav { get; set; } = null!;

    public virtual BProsjekt Prosjekt { get; set; } = null!;
}
