using System.ComponentModel;

namespace SeaEco.Abstractions.Enums;

public enum Prosjektstatus
{
    [Description("Nytt")]
    Nytt = 1,

    [Description("Påbegynt")]
    Pabegynt = 2,

    [Description("Ferdig")]
    Ferdig = 3,

    [Description("Deaktivert")]
    Deaktivert = 4,
    
    [Description("Pågår")]
    Pagar = 5
}