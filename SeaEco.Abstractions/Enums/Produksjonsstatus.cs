using System.ComponentModel;

namespace SeaEco.Abstractions.Enums;

public enum Produksjonsstatus
{
    [Description("Før utsett")]
    ForUtsett = 1,

    [Description("Halv belastning")]
    HalvBelastning = 2,

    [Description("Maks belastning")]
    MaksBelastning = 3,

    [Description("Annet")]
    Annet = 4
}