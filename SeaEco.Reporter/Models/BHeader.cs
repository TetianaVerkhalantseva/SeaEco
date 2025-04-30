namespace SeaEco.Reporter.Models;

public sealed class BHeader
{
    public int LokalitetsID { get; set; } 

    public string Oppdragsgiver { get; set; } = string.Empty;
    public string Lokalitetsnavn { get; set; } = string.Empty;

    public IEnumerable<DateTime> FeltDatoer { get; set; } = [];
    
    
    //public float pHSjo { get; set; } //b_sjovann
    //public float EhSjo { get; set; } //b_sjovann
    //public float SjoTemperatur { get; set; } // b_sjovann BufferTemperatur = SjoTemperatur
    //public float SedimentTemperatur { get; set; } //b_sediment
    //public float RefElektrode { get; set; } // b_preinfo
}