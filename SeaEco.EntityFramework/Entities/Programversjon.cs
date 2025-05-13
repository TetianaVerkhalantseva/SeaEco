using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class Programversjon
{
    public Guid Id { get; set; }

    public DateOnly Utgivelsesdato { get; set; }

    public string Versjonsnummer { get; set; } = null!;

    public string Forbedringer { get; set; } = null!;

    public bool ErAktiv { get; set; }
}
