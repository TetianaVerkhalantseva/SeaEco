namespace SeaEco.Abstractions.Extensions;

public static class ReportExtensions
{
    public static string GetBunnTypeLabel(Guid? blotbunnId, Guid? hardbunnId)
    {
        if (blotbunnId.HasValue && !hardbunnId.HasValue) return "B";
        if (hardbunnId.HasValue && !blotbunnId.HasValue) return "H";

        throw new InvalidOperationException("Ugyldig bunntype: begge eller ingen ID-er er satt.");
    }
}