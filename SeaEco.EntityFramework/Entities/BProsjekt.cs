using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BProsjekt
{
    public int Prosjektid { get; set; }

    public int Kundeid { get; set; }

    public string Kundekontaktpersons { get; set; } = null!;

    public int Kundetlf { get; set; }

    public string Kundeepost { get; set; } = null!;

    public int Lokalitetid { get; set; }

    public string Lokalitet { get; set; } = null!;

    public int Antallstasjoner { get; set; }

    public int Mtbtillatelse { get; set; }

    public int Biomasse { get; set; }

    public int Ansvarligansattid { get; set; }

    public int? Ansvarligansatt2id { get; set; }

    public int? Ansvarligansatt3id { get; set; }

    public int? Ansvarligansatt4id { get; set; }

    public int? Ansvarligansatt5id { get; set; }

    public DateOnly Planlagtfeltdato { get; set; }

    public string? Merknad { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? Datoregistrert { get; set; }

    public virtual Ansatte Ansvarligansatt { get; set; } = null!;

    public virtual Ansatte? Ansvarligansatt2 { get; set; }

    public virtual Ansatte? Ansvarligansatt3 { get; set; }

    public virtual Ansatte? Ansvarligansatt4 { get; set; }

    public virtual Ansatte? Ansvarligansatt5 { get; set; }

    public virtual BProsjektUtstyr? BProsjektUtstyr { get; set; }

    public virtual BProvetakingsplan? BProvetakingsplan { get; set; }

    public virtual ICollection<BRapport> BRapports { get; set; } = new List<BRapport>();

    public virtual ICollection<BStasjon> BStasjons { get; set; } = new List<BStasjon>();

    public virtual Kunde Kunde { get; set; } = null!;
}
