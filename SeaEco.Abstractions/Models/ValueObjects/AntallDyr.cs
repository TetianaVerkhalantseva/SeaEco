namespace SeaEco.Abstractions.Models.ValueObjects;

public class AntallDyr
{
    public string? Verdi { get; private set; }

    public AntallDyr(string? verdi)
    {
        if (string.IsNullOrWhiteSpace(verdi))
        {
            Verdi = string.Empty;
            return;
        }

        if (verdi == "30+" || verdi == "50+")
        {
            Verdi = verdi;
            return;
        }

        if (int.TryParse(verdi, out int antall) && antall >= 0 && antall <= 30)
        {
            Verdi = verdi;
            return;
        }

        throw new ArgumentException($"Ugyldig verdi for antall dyr: '{verdi}'", nameof(verdi));
    }

    public static AntallDyr FraTall(int antall)
    {
        if (antall < 0 || antall > 30)
            throw new ArgumentOutOfRangeException(nameof(antall));
        return new AntallDyr(antall.ToString());
    }

    public static AntallDyr Over30() => new AntallDyr("30+");

    public static AntallDyr Over50() => new AntallDyr("50+");

    public override string ToString() => Verdi ?? string.Empty;
}