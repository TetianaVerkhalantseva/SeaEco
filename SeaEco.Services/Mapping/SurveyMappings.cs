using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Bundersokelse;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.Mapping;

public static class SurveyMappings
{
    public static SurveyDto ToSurveyDto(this BUndersokelse entity)
    {
        return new SurveyDto
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
            
            BPreInfo = entity.Preinfo != null ? new BPreInfoDto
            {
                Id = entity.Preinfo.Id,
                ProsjektId = entity.Preinfo.ProsjektId,
                Feltdato = entity.Preinfo.Feltdato,
                FeltansvarligId = entity.Preinfo.FeltansvarligId
            } : null!,

            BSoftBase = entity.Blotbunn != null ? new BSoftBaseDto
            {
                Id = entity.Blotbunn.Id,
                Leire = entity.Blotbunn.Leire,
                Silt = entity.Blotbunn.Silt,
                Sand = entity.Blotbunn.Sand,
                Grus = entity.Blotbunn.Grus,
                Skjellsand = entity.Blotbunn.Skjellsand
                
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

            BBilders = entity.BBilders.Select(b => new BPictureDto
            {
                Id = b.Id,
                UndersokelseId = b.UndersokelseId,
                Silt = b.Silt,
                Extension = b.Extension
            }).ToList(),

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
}