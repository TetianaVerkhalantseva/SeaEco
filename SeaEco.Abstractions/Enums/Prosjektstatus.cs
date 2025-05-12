using System.ComponentModel;

namespace SeaEco.Abstractions.Enums;

public enum Prosjektstatus
{
    [Description("Nytt")]
    Nytt = 1,

    [Description("Påbegynt")]
    Pabegynt = 2,

    [Description("Pågår")]
    Pagar= 3,

    [Description("Ferdig")]
    Ferdig = 4,
    
    [Description("Deaktivert")]
    Deaktivert = 5
}