using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BDyr
{
    public Guid ProsjektId { get; set; }

    public Guid StasjonsId { get; set; }

    public int? Antallpigghunder { get; set; }

    public int? Antallkrepsdyr { get; set; }

    public int? Antallskjell { get; set; }

    public int? Antallborstemark { get; set; }

    public bool? Beggiota { get; set; }

    public bool? Foor { get; set; }

    public bool? Fekalier { get; set; }

    public virtual BStasjon BStasjon { get; set; } = null!;
}
