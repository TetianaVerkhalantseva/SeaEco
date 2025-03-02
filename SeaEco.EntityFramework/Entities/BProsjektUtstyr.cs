using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BProsjektUtstyr
{
    public int Prosjektid { get; set; }

    public int Grabbid { get; set; }

    public int Phehmeter { get; set; }

    public DateOnly Datokalibrert { get; set; }

    public int Silid { get; set; }

    public DateTime? Datoregistrert { get; set; }

    public virtual BProsjekt Prosjekt { get; set; } = null!;
}
