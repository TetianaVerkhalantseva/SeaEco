using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BRapporter
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public int ArkNavn { get; set; }

    public DateTime Datogenerert { get; set; }

    public virtual BProsjekt Prosjekt { get; set; } = null!;
}
