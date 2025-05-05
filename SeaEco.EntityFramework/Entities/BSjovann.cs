using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BSjovann
{
    public Guid Id { get; set; }

    public Guid PreinfoId { get; set; }

    public float Ph { get; set; }

    public float Eh { get; set; }

    public float Temperatur { get; set; }

    public float? RefElektrode { get; set; }

    public virtual BPreinfo Preinfo { get; set; } = null!;
}
