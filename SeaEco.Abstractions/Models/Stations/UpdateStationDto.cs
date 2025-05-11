using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using SeaEco.Abstractions.Resources;

namespace SeaEco.Abstractions.Models.Stations;

public class UpdateStationDto
{
    public Guid Id { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageDepth", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public int Dybde { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageMethod", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string Analyser { get; set; } = "Parameter I, II og III";
    public int Nummer { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageNorthDegree", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public int NorthDegree  { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageNorthMinutes", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public float NorthMinutes { get; set; }

    [JsonPropertyName("koordinatNord")]
    public string KoordinatNord
    {
        get => Compose(NorthDegree, NorthMinutes);
        set => (NorthDegree, NorthMinutes) = Parse(value);
    }
    [Required(ErrorMessageResourceName = "ErrorMessageEastDegree", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public int EastDegree  { get; set; }
    [Required(ErrorMessageResourceName = "ErrorMessageEastMinues", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
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







