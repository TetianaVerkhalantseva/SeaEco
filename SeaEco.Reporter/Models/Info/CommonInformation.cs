using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.ValueObjects.Half;

namespace SeaEco.Reporter.Models.Info;

public sealed class CommonInformation
{
    public string ProsjektIdSe { get; set; } = string.Empty;
    public IEnumerable<DateTime> FeltDatoer { get; set; } = [];
    
    // Table 1
    public int TotalStasjoner { get; set; }
    public int TotalGrabbhugg { get; set; }
    public int Hardbunnsstasjoner { get; set; }
    public int MedDyr { get; set; }
    public int MedPhEh { get; set; }

    // Table 2 
    public float Leire { get; set; }
    public float Silt { get; set; } 
    public float Sand { get; set; } 
    public float Grus { get; set; }
    public float Skjellsand { get; set; }
    public float Steinbunn { get; set; }
    public float Fjellbunn { get; set; }

    // Table 3
    public Tuple<int, int> Tilstand1 { get; set; } = Tuple.Create(0, 0);
    public Tuple<int, int> Tilstand2 { get; set; } = Tuple.Create(0, 0);
    public Tuple<int, int> Tilstand3 { get; set; } = Tuple.Create(0, 0);
    public Tuple<int, int> Tilstand4 { get; set; } = Tuple.Create(0, 0);

    // Table 4
    public float IndeksGr2 { get; set; }
    public Tilstand TilstandGr2 { get; set; }


    public float IndeksGr3 { get; set; }
    public Tilstand TilstandGr3 { get; set; }


    public float LokalitetsIndeks { get; set; }
    public Tilstand LokalitetsTilstand { get; set; }
}