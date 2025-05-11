using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BProvetakningsplan
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public DateOnly? Planlagtfeltdato { get; set; }

    public Guid PlanleggerId { get; set; }

    public virtual ICollection<BStasjon> BStasjons { get; set; } = new List<BStasjon>();

    public virtual Bruker Planlegger { get; set; } = null!;

    public virtual BProsjekt Prosjekt { get; set; } = null!;
}
