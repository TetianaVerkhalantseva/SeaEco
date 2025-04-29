using System.ComponentModel;

namespace SeaEco.Abstractions.Enums;

public enum Arter
{
    [Description("Capitella Capitata")]
    CC = 1,

    [Description("Thyasira sp.")]
    TH = 2,

    [Description("Ophryotrocha sp.")]
    OP = 3,

    [Description("Slangesjøstjerne")]
    SL = 4,

    [Description("Sjømus")]
    SM = 5,

    [Description("Venusskjell")]
    V = 6 
}