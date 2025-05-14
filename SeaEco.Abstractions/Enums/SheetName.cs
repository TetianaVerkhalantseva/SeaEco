using System.ComponentModel;

namespace SeaEco.Abstractions.Enums;

public enum SheetName
{
    [Description("Info")]
    Info = 1,
    
    [Description("Prøvetakningsplan")]
    PTP = 2,

    [Description("Posisjoner")]
    Position = 3,

    [Description("B1 Skjema")]
    B1 = 4,

    [Description("B2 Skjema")]
    B2 = 5,
    
    [Description("Bilder")]
    Image = 6,
    
    [Description("PhEh-Plot")]
    Plot = 7,
}