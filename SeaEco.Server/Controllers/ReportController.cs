using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Enums.Bsensorisk;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Reporter;
using SeaEco.Reporter.Models;
using SeaEco.Reporter.Models.B1;

namespace SeaEco.Server.Controllers;

[Route("api/report")]
public class ReportController(Report report) : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateB1()
    {
        Response<string> response = report.CopyDocument(SheetName.B1);
        if (response.IsError)
        {
            return AsBadRequest(response.ErrorMessage);
        }

        report.FillB1(response.Value,
        [
            new ColumnB1 { Bunntype = Bunntype.Blotbunn, Dyr = Dyr.Ja, pH = 7.2f, Eh = 200.5f, phEh = 1, TilstandProveGr2 = Tilstand.Blue, Gassbobler = Gassbobler.Nei, Farge = Farge.LysGrå, Lukt = Lukt.Ingen, Konsistens = Konsistens.Fast, Grabbvolum = Grabbvolum.MindreEnnKvart, Tykkelseslamlag = Tykkelseslamlag.Under2Cm, Sum = 10, KorrigertSum = 9.5f, TilstandProveGr3 = Tilstand.Green, MiddelVerdiGr2Gr3 = 1.8f, TilstandProveGr2Gr3 = Tilstand.Blue },
            new ColumnB1 { Bunntype = Bunntype.Hardbunn, Dyr = Dyr.Nei, pH = 6.8f, Eh = 180.3f, phEh = 2, TilstandProveGr2 = Tilstand.Green, Gassbobler = Gassbobler.Ja, Farge = Farge.BrunSvart, Lukt = Lukt.Noe, Konsistens = Konsistens.Myk, Grabbvolum = Grabbvolum.MellomKvartOgTreKvart, Tykkelseslamlag = Tykkelseslamlag.Mellom2Og8Cm, Sum = 12, KorrigertSum = 11.2f, TilstandProveGr3 = Tilstand.Yellow, MiddelVerdiGr2Gr3 = 2.5f, TilstandProveGr2Gr3 = Tilstand.Green },
            new ColumnB1 { Bunntype = Bunntype.Blotbunn, Dyr = Dyr.Ja, pH = 7.0f, Eh = 190.0f, phEh = 3, TilstandProveGr2 = Tilstand.Yellow, Gassbobler = Gassbobler.Nei, Farge = Farge.LysGrå, Lukt = Lukt.Ingen, Konsistens = Konsistens.Fast, Grabbvolum = Grabbvolum.StørreEnnTreKvart, Tykkelseslamlag = Tykkelseslamlag.Over8Cm, Sum = 15, KorrigertSum = 14.5f, TilstandProveGr3 = Tilstand.Red, MiddelVerdiGr2Gr3 = 3.1f, TilstandProveGr2Gr3 = Tilstand.Red },
            new ColumnB1 { Bunntype = Bunntype.Hardbunn, Dyr = Dyr.Nei, pH = 6.5f, Eh = 170.0f, phEh = 2, TilstandProveGr2 = Tilstand.Green, Gassbobler = Gassbobler.Ja, Farge = Farge.BrunSvart, Lukt = Lukt.Noe, Konsistens = Konsistens.Myk, Grabbvolum = Grabbvolum.MellomKvartOgTreKvart, Tykkelseslamlag = Tykkelseslamlag.Mellom2Og8Cm, Sum = 11, KorrigertSum = 10.8f, TilstandProveGr3 = Tilstand.Yellow, MiddelVerdiGr2Gr3 = 2.3f, TilstandProveGr2Gr3 = Tilstand.Green },
            new ColumnB1 { Bunntype = Bunntype.Blotbunn, Dyr = Dyr.Ja, pH = 7.3f, Eh = 210.0f, phEh = 1, TilstandProveGr2 = Tilstand.Blue, Gassbobler = Gassbobler.Nei, Farge = Farge.LysGrå, Lukt = Lukt.Ingen, Konsistens = Konsistens.Fast, Grabbvolum = Grabbvolum.MindreEnnKvart, Tykkelseslamlag = Tykkelseslamlag.Under2Cm, Sum = 9, KorrigertSum = 8.7f, TilstandProveGr3 = Tilstand.Green, MiddelVerdiGr2Gr3 = 1.6f, TilstandProveGr2Gr3 = Tilstand.Blue },
            new ColumnB1 { Bunntype = Bunntype.Hardbunn, Dyr = Dyr.Nei, pH = 6.9f, Eh = 185.0f, phEh = 2, TilstandProveGr2 = Tilstand.Green, Gassbobler = Gassbobler.Ja, Farge = Farge.BrunSvart, Lukt = Lukt.Noe, Konsistens = Konsistens.Myk, Grabbvolum = Grabbvolum.MellomKvartOgTreKvart, Tykkelseslamlag = Tykkelseslamlag.Mellom2Og8Cm, Sum = 13, KorrigertSum = 12.5f, TilstandProveGr3 = Tilstand.Yellow, MiddelVerdiGr2Gr3 = 2.7f, TilstandProveGr2Gr3 = Tilstand.Green },
            new ColumnB1 { Bunntype = Bunntype.Blotbunn, Dyr = Dyr.Ja, pH = 7.1f, Eh = 195.0f, phEh = 3, TilstandProveGr2 = Tilstand.Yellow, Gassbobler = Gassbobler.Nei, Farge = Farge.LysGrå, Lukt = Lukt.Ingen, Konsistens = Konsistens.Fast, Grabbvolum = Grabbvolum.StørreEnnTreKvart, Tykkelseslamlag = Tykkelseslamlag.Over8Cm, Sum = 14, KorrigertSum = 13.8f, TilstandProveGr3 = Tilstand.Red, MiddelVerdiGr2Gr3 = 3.0f, TilstandProveGr2Gr3 = Tilstand.Red },
            new ColumnB1 { Bunntype = Bunntype.Hardbunn, Dyr = Dyr.Nei, pH = 6.6f, Eh = 175.0f, phEh = 2, TilstandProveGr2 = Tilstand.Green, Gassbobler = Gassbobler.Ja, Farge = Farge.BrunSvart, Lukt = Lukt.Noe, Konsistens = Konsistens.Myk, Grabbvolum = Grabbvolum.MellomKvartOgTreKvart, Tykkelseslamlag = Tykkelseslamlag.Mellom2Og8Cm, Sum = 10, KorrigertSum = 9.8f, TilstandProveGr3 = Tilstand.Yellow, MiddelVerdiGr2Gr3 = 2.2f, TilstandProveGr2Gr3 = Tilstand.Green },
            new ColumnB1 { Bunntype = Bunntype.Blotbunn, Dyr = Dyr.Ja, pH = 7.4f, Eh = 220.0f, phEh = 1, TilstandProveGr2 = Tilstand.Blue, Gassbobler = Gassbobler.Nei, Farge = Farge.LysGrå, Lukt = Lukt.Ingen, Konsistens = Konsistens.Fast, Grabbvolum = Grabbvolum.MindreEnnKvart, Tykkelseslamlag = Tykkelseslamlag.Under2Cm, Sum = 8, KorrigertSum = 7.8f, TilstandProveGr3 = Tilstand.Green, MiddelVerdiGr2Gr3 = 1.4f, TilstandProveGr2Gr3 = Tilstand.Blue },
            new ColumnB1 { Bunntype = Bunntype.Hardbunn, Dyr = Dyr.Nei, pH = 6.7f, Eh = 180.0f, phEh = 2, TilstandProveGr2 = Tilstand.Green, Gassbobler = Gassbobler.Ja, Farge = Farge.BrunSvart, Lukt = Lukt.Noe, Konsistens = Konsistens.Myk, Grabbvolum = Grabbvolum.MellomKvartOgTreKvart, Tykkelseslamlag = Tykkelseslamlag.Mellom2Og8Cm, Sum = 12, KorrigertSum = 11.5f, TilstandProveGr3 = Tilstand.Yellow, MiddelVerdiGr2Gr3 = 2.6f, TilstandProveGr2Gr3 = Tilstand.Green },
            new ColumnB1 { Bunntype = Bunntype.Blotbunn, Dyr = Dyr.Ja, pH = 7.5f, Eh = 230.0f, phEh = 1, TilstandProveGr2 = Tilstand.Blue, Gassbobler = Gassbobler.Nei, Farge = Farge.LysGrå, Lukt = Lukt.Ingen, Konsistens = Konsistens.Fast, Grabbvolum = Grabbvolum.MindreEnnKvart, Tykkelseslamlag = Tykkelseslamlag.Under2Cm, Sum = 7, KorrigertSum = 6.8f, TilstandProveGr3 = Tilstand.Green, MiddelVerdiGr2Gr3 = 1.2f, TilstandProveGr2Gr3 = Tilstand.Blue },
            new ColumnB1 { Bunntype = Bunntype.Hardbunn, Dyr = Dyr.Nei, pH = 6.4f, Eh = 165.0f, phEh = 2, TilstandProveGr2 = Tilstand.Green, Gassbobler = Gassbobler.Ja, Farge = Farge.BrunSvart, Lukt = Lukt.Noe, Konsistens = Konsistens.Myk, Grabbvolum = Grabbvolum.MellomKvartOgTreKvart, Tykkelseslamlag = Tykkelseslamlag.Mellom2Og8Cm, Sum = 9, KorrigertSum = 8.5f, TilstandProveGr3 = Tilstand.Yellow, MiddelVerdiGr2Gr3 = 2.0f, TilstandProveGr2Gr3 = Tilstand.Green },
            new ColumnB1 { Bunntype = Bunntype.Blotbunn, Dyr = Dyr.Ja, pH = 7.6f, Eh = 240.0f, phEh = 1, TilstandProveGr2 = Tilstand.Blue, Gassbobler = Gassbobler.Nei, Farge = Farge.LysGrå, Lukt = Lukt.Ingen, Konsistens = Konsistens.Fast, Grabbvolum = Grabbvolum.MindreEnnKvart, Tykkelseslamlag = Tykkelseslamlag.Under2Cm, Sum = 6, KorrigertSum = 5.8f, TilstandProveGr3 = Tilstand.Green, MiddelVerdiGr2Gr3 = 1.0f, TilstandProveGr2Gr3 = Tilstand.Blue },
            new ColumnB1 { Bunntype = Bunntype.Hardbunn, Dyr = Dyr.Nei, pH = 6.3f, Eh = 160.0f, phEh = 2, TilstandProveGr2 = Tilstand.Green, Gassbobler = Gassbobler.Ja, Farge = Farge.BrunSvart, Lukt = Lukt.Noe, Konsistens = Konsistens.Myk, Grabbvolum = Grabbvolum.MellomKvartOgTreKvart, Tykkelseslamlag = Tykkelseslamlag.Mellom2Og8Cm, Sum = 8, KorrigertSum = 7.5f, TilstandProveGr3 = Tilstand.Yellow, MiddelVerdiGr2Gr3 = 1.8f, TilstandProveGr2Gr3 = Tilstand.Green }
        ],
        new BHeader()
        {
            
        });

        return AsOk();
    }
}