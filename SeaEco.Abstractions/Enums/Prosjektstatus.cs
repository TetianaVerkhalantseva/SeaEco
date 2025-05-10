using System.ComponentModel;

namespace SeaEco.Abstractions.Enums;

public enum Prosjektstatus
{
    [Description("Nytt")]
    Nytt = 1,

    [Description("Påbegynt")]
    Pabegynt = 2,

    [Description("Pågår")]
    Ferdig = 3,

    [Description("Ferdig")]
    Deaktivert = 4,
    
    [Description("Deaktivert")]
    Pagar = 5
}