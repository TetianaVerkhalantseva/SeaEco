using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class Token
{
    public Guid Id { get; set; }

    public string Token1 { get; set; } = null!;

    public bool IsUsed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UsedAt { get; set; }

    public DateTime ExpiredAt { get; set; }

    public Guid BrukerId { get; set; }

    public virtual Bruker Bruker { get; set; } = null!;
}
