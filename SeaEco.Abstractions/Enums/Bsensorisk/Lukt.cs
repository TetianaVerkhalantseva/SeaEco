using System.ComponentModel;

namespace SeaEco.Abstractions.Enums.Bsensorisk;

public enum Lukt
{
    [Description("Ingen")]
    Ingen = 0,

    [Description("Noe")]
    Noe = 2,

    [Description("Sterk")]
    Sterk = 4
}