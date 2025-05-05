using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BUndersokelseslogg
{
    public Guid Id { get; set; }

    public string Felt { get; set; } = null!;

    public string GammelVerdi { get; set; } = null!;

    public string NyVerdi { get; set; } = null!;

    public DateTime DatoEndret { get; set; }

    public Guid EndretAv { get; set; }

    public Guid UndersokelseId { get; set; }

    public virtual Bruker EndretAvNavigation { get; set; } = null!;

    public virtual BUndersokelse Undersokelse { get; set; } = null!;
}
