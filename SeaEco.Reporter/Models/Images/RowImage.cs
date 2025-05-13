namespace SeaEco.Reporter.Models.Images;

public class RowImage
{
    public int Nummer { get; set; }

    public byte[]? UsiltImage { get; set; } = [];
    public byte[]? SiltImage { get; set; } = [];
}