using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BUndersokelse
{
    public Guid Id { get; set; }

    public Guid ProsjektId { get; set; }

    public Guid PreinfoId { get; set; }

    public DateOnly Feltdato { get; set; }

    public int? AntallGrabbhugg { get; set; }

    public bool GrabbhastighetGodkjent { get; set; }

    public Guid? BlotbunnId { get; set; }

    public Guid? HardbunnId { get; set; }

    public Guid? SedimentId { get; set; }

    public Guid? SensoriskId { get; set; }

    public bool Beggiatoa { get; set; }

    public bool Forrester { get; set; }

    public bool Fekalier { get; set; }

    public Guid? DyrId { get; set; }

    public string? Merknader { get; set; }

    public DateTime? DatoRegistrert { get; set; }

    public DateTime? DatoEndret { get; set; }

    public float? IndeksGr2Gr3 { get; set; }

    public int? TilstandGr2Gr3 { get; set; }

    public virtual ICollection<BBilder> BBilders { get; set; } = new List<BBilder>();

    public virtual BStasjon? BStasjon { get; set; }

    public virtual ICollection<BUndersokelseslogg> BUndersokelsesloggs { get; set; } = new List<BUndersokelseslogg>();

    public virtual BBlotbunn? Blotbunn { get; set; }

    public virtual BDyr? Dyr { get; set; }

    public virtual BHardbunn? Hardbunn { get; set; }

    public virtual BPreinfo Preinfo { get; set; } = null!;

    public virtual BProsjekt Prosjekt { get; set; } = null!;

    public virtual BSediment? Sediment { get; set; }

    public virtual BSensorisk? Sensorisk { get; set; }
}
