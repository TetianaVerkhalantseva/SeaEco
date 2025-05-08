using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class Kunde
{
    public Guid Id { get; set; }

    public string Oppdragsgiver { get; set; } = null!;

    public string Kontaktperson { get; set; } = null!;

    public string Telefon { get; set; } = null!;

    public virtual ICollection<BProsjekt> BProsjekts { get; set; } = new List<BProsjekt>();
}
