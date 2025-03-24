using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BBilder
{
    public int Bildeid { get; set; }

    public string Posisjon { get; set; } = null!;

    public Guid Prosjektid { get; set; }

    public int Stasjonsid { get; set; }

    public byte[] Bilde { get; set; } = null!;

    public DateTime Datoregistrert { get; set; }

    public virtual BStasjon BStasjon { get; set; } = null!;
}
