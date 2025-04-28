namespace SeaEco.Abstractions.ValueObjects;

public readonly struct Coordinate
{
    public int NorthDegree { get; }
    public float NorthMinutes { get; }
    public int EastDegree { get; }
    public float EastMinutes { get; }

    public string North => $"{NorthDegree}°{NorthMinutes:F3}";
    public string East => $"{EastDegree}°{EastMinutes:F3}";

    public Coordinate(int northDegree, float northMinutes, int eastDegree, float eastMinutes)
    {
        NorthDegree = northDegree;
        NorthMinutes = northMinutes;
        EastDegree = eastDegree;
        EastMinutes = eastMinutes;
    }
}