using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BBilder
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Posisjon { get; set; } = null!;
    
    public Guid Extension { get; set; }

    public Guid Prosjektid { get; set; }

    public Guid Stasjonsid { get; set; }

    public DateTime Datoregistrert { get; set; }

    public virtual BStasjon BStasjon { get; set; } = null!;
}