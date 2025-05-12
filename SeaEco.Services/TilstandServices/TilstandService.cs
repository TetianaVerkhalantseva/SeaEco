using SeaEco.Abstractions.ResponseService;

namespace SeaEco.Services.TilstandServices;

public sealed class TilstandService
{ 
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
}