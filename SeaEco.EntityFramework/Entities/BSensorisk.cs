using System;
using System.Collections.Generic;

namespace SeaEco.EntityFramework.Entities;

public partial class BSensorisk
{
    public int Prosjektid { get; set; }

    public int Stasjonsid { get; set; }

    public int ProvePh { get; set; }

    public int ProveEh { get; set; }

    public int ProveTemperatur { get; set; }

    public bool Farge { get; set; }

    public int Lukt { get; set; }

    public int Konsistens { get; set; }

    public int Grabbvolum { get; set; }

    public int Tykkelseslamlag { get; set; }

    public int Gassbobler { get; set; }

    public virtual BStasjon BStasjon { get; set; } = null!;

    public virtual SysFarge FargeNavigation { get; set; } = null!;

    public virtual SysGassbobler GassboblerNavigation { get; set; } = null!;

    public virtual SysGrabbvolum GrabbvolumNavigation { get; set; } = null!;

    public virtual SysKonsisten KonsistensNavigation { get; set; } = null!;

    public virtual SysLukt LuktNavigation { get; set; } = null!;

    public virtual SysTykkelsepaslam TykkelseslamlagNavigation { get; set; } = null!;
}
