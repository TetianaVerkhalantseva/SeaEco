using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BHardbunn
{
    public Guid Id { get; set; }

    public float Steinbunn { get; set; }

    public float Fjellbunn { get; set; }

    public virtual BUndersokelse? BUndersokelse { get; set; }
}
