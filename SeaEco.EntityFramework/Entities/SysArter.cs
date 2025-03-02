using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class SysArter
{
    public int Artid { get; set; }

    public string Artsforkortelse { get; set; } = null!;

    public string Artsnavn { get; set; } = null!;
}
