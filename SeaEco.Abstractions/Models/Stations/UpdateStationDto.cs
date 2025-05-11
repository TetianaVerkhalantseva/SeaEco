using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SeaEco.Abstractions.Models.Stations;

public class UpdateStationDto
{
    public Guid Id { get; set; }
    public int Dybde { get; set; }
    public string Analyser { get; set; } = "Parameter I, II og III";
    public int Nummer { get; set; }
    public int NorthDegree  { get; set; }
    public float NorthMinutes { get; set; }

    [JsonPropertyName("koordinatNord")]
    public string KoordinatNord
    {
        get => Compose(NorthDegree, NorthMinutes);
        set => (NorthDegree, NorthMinutes) = Parse(value);
    }
    
    public int EastDegree  { get; set; }
    public float EastMinutes { get; set; }

    [JsonPropertyName("koordinatOst")]
    public string KoordinatOst
    {
        get => Compose(EastDegree, EastMinutes);
        set => (EastDegree, EastMinutes) = Parse(value);
    }
    
    private static string Compose(int deg, float min) => $"{deg}\u00b0{min.ToString("0.####", CultureInfo.InvariantCulture)}";

    private static (int deg, float min) Parse(string coordinates)
    {
        var m = Regex.Match(coordinates, @"^(?<d>\d+).*?(?<m>\d+([.,]\d+)?)",RegexOptions.CultureInvariant);
        var d = int.Parse(m.Groups["d"].Value, CultureInfo.InvariantCulture);
        var mnt = float.Parse(m.Groups["m"].Value, CultureInfo.InvariantCulture);
        return (d, mnt);
    }
}







