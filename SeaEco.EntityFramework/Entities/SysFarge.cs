using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class SysFarge
{
    public int Id { get; set; }

    public string Beskrivelse { get; set; } = null!;

    public bool Verdi { get; set; }

    public virtual ICollection<BSensorisk> BSensorisks { get; set; } = new List<BSensorisk>();
}
