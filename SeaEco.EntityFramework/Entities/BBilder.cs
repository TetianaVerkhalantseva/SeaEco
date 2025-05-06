using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BBilder
{
    public Guid Id { get; set; }

    public Guid UndersokelseId { get; set; }

    public bool Silt { get; set; }

    public string Extension { get; set; } = null!;

    public DateTime Datogenerert { get; set; }

    public virtual BUndersokelse Undersokelse { get; set; } = null!;
}
