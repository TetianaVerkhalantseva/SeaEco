using System.ComponentModel;

namespace SeaEco.Abstractions.Enums;

public enum SheetName
{
    [Description("Info")]
    Info = 1,

    [Description("Posisjoner")]
    Position = 2,

    [Description("B1 Skjema")]
    B1 = 3,

    [Description("B2 Skjema")]
    B2 = 4,
}