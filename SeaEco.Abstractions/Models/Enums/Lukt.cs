using System.ComponentModel;

namespace SeaEco.Abstractions.Models.Enums;

public enum Lukt
{
    [Description("Ingen")]
    Ingen = 0,

    [Description("Noe")]
    Noe = 2,

    [Description("Sterk")]
    Sterk = 4
}