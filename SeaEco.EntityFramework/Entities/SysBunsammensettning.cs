using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class SysBunsammensettning
{
    public int Bunnsammensettningid { get; set; }

    public bool? Sand { get; set; }

    public bool? Leire { get; set; }

    public bool? Silt { get; set; }

    public bool? Grus { get; set; }

    public bool? Skjellsand { get; set; }

    public bool? Steinbunn { get; set; }

    public bool? Fjellbunn { get; set; }

    public virtual ICollection<BStasjon> BStasjons { get; set; } = new List<BStasjon>();
}
