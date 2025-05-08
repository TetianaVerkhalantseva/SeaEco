using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class Lokalitet
{
    public Guid Id { get; set; }

    public string Lokalitetsnavn { get; set; } = null!;

    public string LokalitetsId { get; set; } = null!;

    public virtual ICollection<BProsjekt> BProsjekts { get; set; } = new List<BProsjekt>();
}
