using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class SysKonsisten
{
    public int Id { get; set; }

    public string Beskrivelse { get; set; } = null!;

    public int Verdi { get; set; }

    public virtual ICollection<BSensorisk> BSensorisks { get; set; } = new List<BSensorisk>();
}
