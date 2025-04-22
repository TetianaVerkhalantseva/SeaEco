using System.ComponentModel.DataAnnotations.Schema;
using SeaEco.Abstractions.ValueObjects;

namespace SeaEco.Abstractions.Models.Bundersokelse;

public class BdyrDto
{
    public string Pigghunder { get; set; }

    public string Krepsdyr { get; set; }

    public string Skjell { get; set; }

    public string Børstemark { get; set; }

    [NotMapped]
    public AntallDyr PigghunderAntall
    {
        get => new AntallDyr(Pigghunder);
        set => Pigghunder = value.ToString();
    }

    [NotMapped]
    public AntallDyr KrepsdyrAntall
    {
        get => new AntallDyr(Krepsdyr);
        set => Krepsdyr = value.ToString();
    }

    [NotMapped]
    public AntallDyr SkjellAntall
    {
        get => new AntallDyr(Skjell);
        set => Skjell = value.ToString();
    }

    [NotMapped]
    public AntallDyr BørstemarkAntall
    {
        get => new AntallDyr(Børstemark);
        set => Børstemark = value.ToString();
    }
}