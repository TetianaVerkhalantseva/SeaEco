using System.ComponentModel;

namespace SeaEco.Abstractions.Models.Enums;

public enum Tykkelseslamlag
{
    [Description("0-2 cm")]
    Under2Cm = 0,

    [Description("2-8 cm")]
    Mellom2Og8Cm = 1,

    [Description(">8 cm")]
    Over8Cm = 2
}