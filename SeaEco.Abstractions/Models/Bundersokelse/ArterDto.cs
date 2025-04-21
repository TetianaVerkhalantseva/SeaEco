using System.ComponentModel.DataAnnotations.Schema;
using SeaEco.Abstractions.Extensions;
using SeaEco.Abstractions.Models.Enums;

namespace SeaEco.Abstractions.Models.Bundersokelse;

public class ArterDto
{
    public string Value { get; set; } = string.Empty;

    [NotMapped]
    public List<Arter> Selected { get; set; } = new();

    [NotMapped]
    public string? Comment { get; set; }

    public void Parse()
    {
        if (string.IsNullOrWhiteSpace(Value)) return;

        var parts = Value.Split('|');
        if (parts.Length > 0)
        {
            Selected = parts[0]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Enum.TryParse<Arter>(s.Trim(), out var val) ? val : default)
                .Where(x => Enum.IsDefined(typeof(Arter), x))
                .ToList();
        }

        if (parts.Length > 1)
            Comment = parts[1].Trim();
    }

    public void Build()
    {
        var arterPart = string.Join(",", Selected.Select(a => a.ToString()));
        Value = string.IsNullOrWhiteSpace(Comment) ? arterPart : $"{arterPart}|{Comment}";
    }

    public string Display =>
        string.Join(", ", Selected.Select(a => a.GetDescription()))
        + (string.IsNullOrWhiteSpace(Comment) ? "" : $", {Comment}");
}
