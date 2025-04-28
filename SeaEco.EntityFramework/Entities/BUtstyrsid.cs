using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BUtstyrsid
{
    public Guid Id { get; set; }

    public Guid PreinfoId { get; set; }

    public string Grabb { get; set; } = null!;

    public string Sil { get; set; } = null!;

    public string PhMeter { get; set; } = null!;

    public DateOnly Kalibreringsdato { get; set; }

    public virtual BPreinfo Preinfo { get; set; } = null!;
}
