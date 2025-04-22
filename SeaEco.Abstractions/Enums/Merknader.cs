using System.ComponentModel;

namespace SeaEco.Abstractions.Enums;

public enum Merknader
{
    [Description("Detritus")]
    D = 1,

    [Description("Terrestrisk materiale")]
    TM = 2,

    [Description("Rester etter anleggsrens")]
    RAR = 3
}