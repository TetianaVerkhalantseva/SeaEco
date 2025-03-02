using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class Kunde
{
    public int Kundeid { get; set; }

    public string Oppdragsgiver { get; set; } = null!;

    public string Kontaktperson { get; set; } = null!;

    public int Orgnr { get; set; }

    public string Postadresse { get; set; } = null!;

    public string Kommune { get; set; } = null!;

    public string Fylke { get; set; } = null!;

    public virtual ICollection<BProsjekt> BProsjekts { get; set; } = new List<BProsjekt>();
}
