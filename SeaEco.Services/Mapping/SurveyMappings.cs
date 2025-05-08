using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Bundersokelse;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.Mapping;

public static class SurveyMappings
{
    public static EditSurveyDto ToSurveyDto(this BUndersokelse entity)
    {
        return new EditSurveyDto
        {
            Id = entity.Id,
            ProsjektId = entity.ProsjektId,
            PreinfoId = entity.PreinfoId,
            Feltdato = entity.Feltdato,
            AntallGrabbhugg = entity.AntallGrabbhugg,
            GrabbhastighetGodkjent = entity.GrabbhastighetGodkjent,
            BlotbunnId = entity.BlotbunnId,
            HardbunnId = entity.HardbunnId,
            SedimentId = entity.SedimentId,
            SensoriskId = entity.SensoriskId,
            Beggiatoa = entity.Beggiatoa,
            Forrester = entity.Forrester,
            Fekalier = entity.Fekalier,
            DyrId = entity.DyrId,
            Merknader = entity.Merknader,
            DatoRegistrert = entity.DatoRegistrert,
            DatoEndret = entity.DatoEndret,
            IndeksGr2Gr3 = entity.IndeksGr2Gr3,
            TilstandGr2Gr3 = entity.TilstandGr2Gr3,
            
            BPreInfo = new BPreInfoDto
            {
                Id = entity.Preinfo.Id,
                ProsjektId = entity.Preinfo.ProsjektId,
                Feltdato = entity.Preinfo.Feltdato,
                FeltansvarligId = entity.Preinfo.FeltansvarligId
            },

            BSoftBase = entity.Blotbunn != null ? new BSoftBaseDto
            {
                Id = entity.Blotbunn.Id,
                Leire = entity.Blotbunn.Leire,
                Silt = entity.Blotbunn.Silt,
                Sand = entity.Blotbunn.Sand,
                Grus = entity.Blotbunn.Grus,
                Skjellsand = entity.Blotbunn.Skjellsand
                
            } : null,
            
            BAnimal = entity.Dyr != null ? new BAnimalDto
            {
                Id = entity.Dyr.Id,
                Pigghunder = entity.Dyr.Pigghunder,
                Krepsdyr = entity.Dyr.Krepsdyr,
                Skjell = entity.Dyr.Skjell,
                Borstemark = entity.Dyr.Borstemark,
                Arter = entity.Dyr.Arter
            } : null,

            BHardBase = entity.Hardbunn != null ? new BHardBaseDto
            {
                Id = entity.Hardbunn.Id,
                Steinbunn = entity.Hardbunn.Steinbunn,
                Fjellbunn = entity.Hardbunn.Fjellbunn
            } : null,

            BSediment = entity.Sediment != null ? new BSedimentDto
            {
                Id = entity.Sediment.Id,
                Ph = entity.Sediment.Ph,
                Eh = entity.Sediment.Eh,
                Temperatur = entity.Sediment.Temperatur,
                KlasseGr2 = entity.Sediment.KlasseGr2,
                TilstandGr2 = entity.Sediment.TilstandGr2
            } : null!,

            BSensorisk = entity.Sensorisk != null ? new BSensoriskDto
            {
                Id = entity.Sensorisk.Id,
                Gassbobler = entity.Sensorisk.Gassbobler,
                Farge = entity.Sensorisk.Farge,
                Lukt = entity.Sensorisk.Lukt,
                Konsistens = entity.Sensorisk.Konsistens,
                Grabbvolum = entity.Sensorisk.Grabbvolum,
                Tykkelseslamlag = entity.Sensorisk.Tykkelseslamlag,
                IndeksGr3 = entity.Sensorisk.IndeksGr3,
                TilstandGr3 = entity.Sensorisk.TilstandGr3
            } : null,

            BStation = entity.BStasjon != null ? new BStationDto
            {
                Id = entity.BStasjon.Id,
                ProsjektId = entity.BStasjon.ProsjektId,
                Nummer = entity.BStasjon.Nummer,
                KoordinatNord = entity.BStasjon.KoordinatNord,
                KoordinatOst = entity.BStasjon.KoordinatOst,
                Dybde = entity.BStasjon.Dybde,
                Analyser = entity.BStasjon.Analyser,
                ProvetakingsplanId = entity.BStasjon.ProvetakingsplanId,
                UndersokelseId = entity.BStasjon.UndersokelseId,
            } : null,

            /*BBilders = entity.BBilders.Select(b => new ImageDto
            {
                Id = b.Id,
                UndersokelseId = b.UndersokelseId,
                Silt = b.Silt,
                Extension = b.Extension
            }).ToList(),*/

            BSurveyLogs = entity.BUndersokelsesloggs.Select(l => new BSurveyLogDto
            {
                Id = l.Id,
                Felt = l.Felt,
                GammelVerdi = l.GammelVerdi,
                NyVerdi = l.NyVerdi,
                DatoEndret = l.DatoEndret,
                EndretAv = l.EndretAv,
                UndersokelseId = l.UndersokelseId
            }).ToList()
        };
    }

    public static BUndersokelse ToEntity(this EditSurveyDto dto)
    {
        return new BUndersokelse
        {
            Id = dto.Id,
            ProsjektId = dto.ProsjektId,
            PreinfoId = dto.PreinfoId,
            Feltdato = dto.Feltdato,
            AntallGrabbhugg = dto.AntallGrabbhugg,
            GrabbhastighetGodkjent = dto.GrabbhastighetGodkjent,
            BlotbunnId = dto.BlotbunnId,
            HardbunnId = dto.HardbunnId,
            SedimentId = dto.SedimentId,
            SensoriskId = dto.SensoriskId,
            Beggiatoa = dto.Beggiatoa,
            Forrester = dto.Forrester,
            Fekalier = dto.Fekalier,
            DyrId = dto.DyrId,
            Merknader = dto.Merknader,
            DatoRegistrert = dto.DatoRegistrert,
            DatoEndret = dto.DatoEndret,
            IndeksGr2Gr3 = dto.IndeksGr2Gr3,
            TilstandGr2Gr3 = dto.TilstandGr2Gr3,

            Blotbunn = dto.BSoftBase != null
                ? new BBlotbunn
                {
                    Id = dto.BSoftBase.Id,
                    Leire = dto.BSoftBase.Leire,
                    Silt = dto.BSoftBase.Silt,
                    Sand = dto.BSoftBase.Sand,
                    Grus = dto.BSoftBase.Grus,
                    Skjellsand = dto.BSoftBase.Skjellsand
                }
                : null,

            Hardbunn = dto.BHardBase != null
                ? new BHardbunn
                {
                    Id = dto.BHardBase.Id,
                    Steinbunn = dto.BHardBase.Steinbunn,
                    Fjellbunn = dto.BHardBase.Fjellbunn
                }
                : null,

            Sediment = dto.BSediment != null
                ? new BSediment
                {
                    Id = dto.BSediment.Id,
                    Ph = dto.BSediment.Ph,
                    Eh = dto.BSediment.Eh,
                    Temperatur = dto.BSediment.Temperatur,
                    KlasseGr2 = dto.BSediment.KlasseGr2,
                    TilstandGr2 = dto.BSediment.TilstandGr2
                }
                : null,

            Sensorisk = dto.BSensorisk != null
                ? new BSensorisk
                {
                    Id = dto.BSensorisk.Id,
                    Gassbobler = dto.BSensorisk.Gassbobler,
                    Farge = dto.BSensorisk.Farge,
                    Lukt = dto.BSensorisk.Lukt,
                    Konsistens = dto.BSensorisk.Konsistens,
                    Grabbvolum = dto.BSensorisk.Grabbvolum,
                    Tykkelseslamlag = dto.BSensorisk.Tykkelseslamlag,
                    IndeksGr3 = dto.BSensorisk.IndeksGr3,
                    TilstandGr3 = dto.BSensorisk.TilstandGr3
                }
                : null,

            Dyr = dto.BAnimal != null
                ? new BDyr
                {
                    Id = dto.BAnimal.Id,
                    Pigghunder = dto.BAnimal.Pigghunder,
                    Krepsdyr = dto.BAnimal.Krepsdyr,
                    Skjell = dto.BAnimal.Skjell,
                    Borstemark = dto.BAnimal.Borstemark,
                    Arter = dto.BAnimal.Arter
                }
                : null,
            
            Preinfo = new BPreinfo
                {
                    Id = dto.BPreInfo.Id,
                    ProsjektId = dto.BPreInfo.ProsjektId,
                    Feltdato = dto.BPreInfo.Feltdato,
                    FeltansvarligId = dto.BPreInfo.FeltansvarligId
                },
            
            BUndersokelsesloggs = dto.BSurveyLogs.Select(log => new BUndersokelseslogg
            {
                Id = log.Id,
                Felt = log.Felt,
                GammelVerdi = log.GammelVerdi,
                NyVerdi = log.GammelVerdi,
                DatoEndret = log.DatoEndret,
                EndretAv = log.EndretAv,
                UndersokelseId = log.UndersokelseId
            }).ToList(),

            BStasjon = dto.BStation != null
                ? new BStasjon
                {
                    Id = dto.BStation.Id,
                    ProsjektId = dto.BStation.ProsjektId,
                    Nummer = dto.BStation.Nummer,
                    KoordinatNord = dto.BStation.KoordinatNord,
                    KoordinatOst = dto.BStation.KoordinatOst,
                    Dybde = dto.BStation.Dybde,
                    Analyser = dto.BStation.Analyser,
                    ProvetakingsplanId = dto.BStation.ProvetakingsplanId,
                    UndersokelseId = dto.BStation.UndersokelseId
                }
                : null,

            /*BBilders = dto.BBilders.Select(pic => new BBilder
            {
                Id = pic.Id,
                UndersokelseId = pic.UndersokelseId,
                Silt = pic.Silt,
                Extension = pic.Extension
            }).ToList()*/
        };
    }
}