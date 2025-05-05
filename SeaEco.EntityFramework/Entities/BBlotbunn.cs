using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BBlotbunn
{
    public Guid Id { get; set; }

    public float Leire { get; set; }

    public float Silt { get; set; }

    public float Sand { get; set; }

    public float Grus { get; set; }

    public float Skjellsand { get; set; }

    public virtual BUndersokelse? BUndersokelse { get; set; }
}
