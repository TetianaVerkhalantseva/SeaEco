using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Enums;

public enum Tilstand
{
    [Description("< 1,1")]
    [Display(Name = "#00B0F0")]
    Blue = 1,

    [Description("1,1 < 2,1")]
    [Display(Name = "#00B050")]
    Green = 2,

    [Description("2,1 < 3,1")]
    [Display(Name = "#FFFF00")]
    Yellow = 3,

    [Description(">= 3,1")]
    [Display(Name = "#FF0000")]
    Red = 4
}