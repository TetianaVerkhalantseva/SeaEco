using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.ValueObjects.Half;

namespace SeaEco.Reporter.Models.Info;

public sealed class CommonInformation
{
    public string Prosjekt { get; set; } = string.Empty;
    public string Dato { get; set; } = string.Empty;

    public int TotalStasjoner { get; set; }
    public int TotalGrabbhugg { get; set; }
    public int Hardbunnsstasjoner { get; set; }
    public int MedDyr { get; set; }
    public int MedPhEh { get; set; }

    public IEnumerable<HalfValue> Leire { get; set; } = [];
    public IEnumerable<HalfValue> Silt { get; set; } = [];
    public IEnumerable<HalfValue> Sand { get; set; } = [];
    public IEnumerable<HalfValue> Grus { get; set; } = [];
    public IEnumerable<HalfValue> Skjellsand { get; set; } = [];
    public IEnumerable<HalfValue> Steinbunn { get; set; } = [];
    public IEnumerable<HalfValue> Fjellbunn { get; set; } = [];

    public Tuple<int, int> Tilstand1 { get; set; } = Tuple.Create(0, 0);
    public Tuple<int, int> Tilstand2 { get; set; } = Tuple.Create(0, 0);
    public Tuple<int, int> Tilstand3 { get; set; } = Tuple.Create(0, 0);
    public Tuple<int, int> Tilstand4 { get; set; } = Tuple.Create(0, 0);

    public Indeks GrII { get; set; }
    public Indeks GrIII { get; set; }
    public Indeks GrII_III { get; set; }
}