using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BTilstand
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public float IndeksGr2 { get; set; }

    public int TilstandGr2 { get; set; }

    public float IndeksGr3 { get; set; }

    public int TilstandGr3 { get; set; }

    public float IndeksLokalitet { get; set; }

    public int TilstandLokalitet { get; set; }

    public virtual BProsjekt Prosjekt { get; set; } = null!;
}
