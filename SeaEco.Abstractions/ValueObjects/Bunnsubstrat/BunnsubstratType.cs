using System.ComponentModel;

namespace SeaEco.Abstractions.ValueObjects.Bunnsubstrat;

public enum BunnsubstratType
{
    [Description("")]
    None = 0,

    //Equal for (x) = 0.5
    [Description("(x)")]
    x = 1,
    
    // Equal for x = 1
    [Description("x")]
    X = 2,
}