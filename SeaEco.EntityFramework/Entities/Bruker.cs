using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class Bruker
{
    public Guid Id { get; set; }

    public string Fornavn { get; set; } = null!;

    public string Etternavn { get; set; } = null!;

    public string Epost { get; set; } = null!;

    public string PassordHash { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public bool Aktiv { get; set; }

    public byte[] Salt { get; set; } = null!;

    public virtual ICollection<BPreinfo> BPreinfos { get; set; } = new List<BPreinfo>();

    public virtual ICollection<BProsjekt> BProsjekts { get; set; } = new List<BProsjekt>();

    public virtual ICollection<BProvetakningsplan> BProvetakningsplans { get; set; } = new List<BProvetakningsplan>();

    public virtual ICollection<BUndersokelseslogg> BUndersokelsesloggs { get; set; } = new List<BUndersokelseslogg>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    public virtual ICollection<BPreinfo> Preinfos { get; set; } = new List<BPreinfo>();
}
