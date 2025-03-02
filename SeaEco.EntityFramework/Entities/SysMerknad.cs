using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class SysMerknad
{
    public int Merknadid { get; set; }

    public string Forkortelse { get; set; } = null!;

    public string Beskrivelse { get; set; } = null!;
}
