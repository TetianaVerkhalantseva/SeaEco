using System.ComponentModel;

namespace SeaEco.Abstractions.Enums.Bsensorisk;

public enum Grabbvolum
{
    [Description("<1/4")]
    MindreEnnKvart = 0,

    [Description("1/4-3/4")]
    MellomKvartOgTreKvart = 1,

    [Description(">3/4")]
    StørreEnnTreKvart = 2
}