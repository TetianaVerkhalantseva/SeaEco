using System.ComponentModel.DataAnnotations.Schema;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Extensions;

namespace SeaEco.Abstractions.Models.Bundersokelse;

public class MerknaderDto
{
    public string Value { get; set; } = string.Empty;

    [NotMapped]
    public List<Merknader> Selected { get; set; } = new();

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
                .Select(s => Enum.TryParse<Merknader>(s.Trim(), out var val) ? val : default)
                .Where(x => Enum.IsDefined(typeof(Merknader), x))
                .ToList();
        }

        if (parts.Length > 1)
            Comment = parts[1].Trim();
    }

    public void Build()
    {
        var codes = string.Join(",", Selected.Select(m => m.ToString()));
        Value = string.IsNullOrWhiteSpace(Comment) ? codes : $"{codes}|{Comment}";
    }

    public string Display =>
        string.Join(", ", Selected.Select(m => m.GetDescription()))
        + (string.IsNullOrWhiteSpace(Comment) ? "" : $", {Comment}");
}
