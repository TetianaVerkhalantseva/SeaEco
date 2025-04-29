using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SeaEco.Abstractions.Enums;

public enum Bunntype
{
    [Description("Bløtbunn")]
    [Display(Name = "B")]
    Blotbunn = 1,

    [Description("Hardbunn")]
    [Display(Name = "H")]
    Hardbunn = 2
}