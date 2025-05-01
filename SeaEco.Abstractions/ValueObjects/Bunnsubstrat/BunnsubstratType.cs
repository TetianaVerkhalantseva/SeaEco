using System.ComponentModel;

namespace SeaEco.Abstractions.ValueObjects.Bunnsubstrat;

public enum BunnsubstratType
{
    [Description("")]
    None = 0,

    //Equal for (x) = 0.5
    [Description("(X)")]
    x = 1,
    
    // Equal for X = 1
    [Description("X")]
    X = 2,
}