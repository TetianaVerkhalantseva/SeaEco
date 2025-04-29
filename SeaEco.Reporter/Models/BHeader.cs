namespace SeaEco.Reporter.Models;

public sealed class BHeader
{
    public int Id { get; set; }

    public string Firma { get; set; } = string.Empty;
    public string Lokalitet { get; set; } = string.Empty;

    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }

    //public float BufferC { get; set; }
    //public float Sjø { get; set; }
    //public float Sediment { get; set; }
    //public float pHSjø { get; set; }
    //public float EhSjø { get; set; }
    //public float RefElektrode { get; set; }
}