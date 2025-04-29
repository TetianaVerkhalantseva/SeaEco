using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BDyr
{
    public Guid Id { get; set; }

    public string? Pigghunder { get; set; }

    public string? Krepsdyr { get; set; }

    public string? Skjell { get; set; }

    public string? Borstemark { get; set; }

    public string? Arter { get; set; }

    public virtual BUndersokelse? BUndersokelse { get; set; }
}
