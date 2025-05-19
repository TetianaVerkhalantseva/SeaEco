using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.TilstandServices;


public sealed class TilstandService
{
    public Response CalculateSedimentTilstand(BSediment sediment)
    {
        Response<int> classResult = CalculateClass(sediment.Ph, sediment.Eh);
        if (classResult.IsError)
        {
            return Response.Error(classResult.ErrorMessage);
        }
        
        sediment.KlasseGr2 = classResult.Value;
        sediment.TilstandGr2 = classResult.Value switch
        {
            0 => (int)Tilstand.Blue,
            1 => (int)Tilstand.Blue,
            2 => (int)Tilstand.Green,
            3 => (int)Tilstand.Yellow,
            5 => (int)Tilstand.Red,
            _ => (int)Tilstand.Blue
        };

        return Response.Ok();
    }

    public void CalculateSensoriskTilstand(BSensorisk sensorisk)
    {
        float index = (sensorisk.Gassbobler +
                       sensorisk.Farge +
                       sensorisk.Lukt +
                       sensorisk.Konsistens +
                       sensorisk.Grabbvolum +
                       sensorisk.Tykkelseslamlag) * 0.22f;
        
        sensorisk.IndeksGr3 = index;
        sensorisk.TilstandGr3 = (int)CalculateTilstand(index);
    }

    public void CalculateUndersokelseTilstand(BUndersokelse undersokelse)
    {
        float index = undersokelse.SedimentId is null &&
                      undersokelse.SensoriskId is not null
                          ? undersokelse.Sensorisk.IndeksGr3 ?? 0f
                          : ((undersokelse.Sediment?.KlasseGr2! ?? 0) +
                           (undersokelse.Sensorisk?.IndeksGr3! ?? 0)) / 2;
                      
        undersokelse.IndeksGr2Gr3 = index;
        undersokelse.TilstandGr2Gr3 = (int)CalculateTilstand(index);
    }

    public BTilstand CalculateProsjektTilstand(IEnumerable<BUndersokelse> undersokelse, Guid projectId)
    {
        float indexGr2Avg = undersokelse.Sum(_ => _.Sediment?.KlasseGr2! ?? 0f) / 
                            (undersokelse.Count() - 
                             undersokelse.Count(_ => _.HardbunnId is not null &&
                                                     _.SedimentId is null &&
                                                     _.SensoriskId is not null)
                             );
        
        float indexGr3Avg = undersokelse.Average(_ => _.Sensorisk?.IndeksGr3! ?? 0);
        float indexGr2Gr3Avg = undersokelse.Average(_ => _.IndeksGr2Gr3! ?? 0);

        return new BTilstand()
        {
            Id = Guid.NewGuid(),
            ProsjektId = projectId,
            IndeksGr2 = indexGr2Avg,
            TilstandGr2 = (int)CalculateTilstand(indexGr2Avg),
            IndeksGr3 = indexGr3Avg,
            TilstandGr3 = (int)CalculateTilstand(indexGr3Avg),
            IndeksLokalitet = indexGr2Gr3Avg,
            TilstandLokalitet = (int)CalculateTilstand(indexGr2Gr3Avg),
        };
    }
    
    public Response<int> CalculateClass(double pH, double eh)
    {

        if (pH < 0 || pH > 10)
        {
            return Response<int>.Error($"pH-verdien på stasjonen er utenfor området [0,10]: {pH}");
        }

        if (pH == 0 && eh == 0)
        {
            return Response<int>.Error("Manglende data – pH- og Eh-verdiene er lik null.");
        }
        
        // Class 0: Eh ≥ 100 or pH ≥ 8.12
        if (eh >= 100 || pH >= 8.12)
        {
            return Response<int>.Ok(0);
        }
        
        // Classes 1 and 2: 7.1 ≤ pH ≤ 8.2 и Eh ≤ 100
        if (pH >= 7.1 && pH <= 8.2 && eh <= 100)
        {
            double metric = eh + 228.57143 * pH;
            return Response<int>.Ok(metric >= 1682.85714 ? 1 : 2);
        }
        
        // Class 3: 6.8 ≤ pH ≤ 7.1 и Eh ≤ 100
        if (pH >= 6.8 && pH <= 7.1 && eh <= 100)
        {
            return Response<int>.Ok(3);
        }
        
        // Class 5: pH ≤ 6.8
        if (pH <= 6.8)
        {
            return Response<int>.Ok(5);
        }
        
        return Response<int>.Error($"Kan ikke kategorisere pH={pH}, Eh={eh}");
    }

    private Tilstand CalculateTilstand(float index)
    {
        if (index < 1.1)
            return Tilstand.Blue;

        if (index >= 1.1 && index < 2.1)
            return Tilstand.Green;

        if (index >= 2.1 && index < 3.1)
            return Tilstand.Yellow;

        if (index >= 3.1)
            return Tilstand.Red;

        return Tilstand.Blue;
    }
}
