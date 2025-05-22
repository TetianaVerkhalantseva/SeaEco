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
            Korrigeringer = entity.Korrigeringer,

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
            Korrigeringer = dto.Korrigeringer ?? "",
            
            Blotbunn = dto.BSoftBase != null
                ? new BBlotbunn
                {
                    Id = dto.BSoftBase.Id == Guid.Empty ? Guid.NewGuid() : dto.BSoftBase.Id,
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
                    Id = dto.BHardBase.Id == Guid.Empty ? Guid.NewGuid() : dto.BHardBase.Id,
                    Steinbunn = dto.BHardBase.Steinbunn,
                    Fjellbunn = dto.BHardBase.Fjellbunn
                }
                : null,

            Sediment = dto.BSediment != null
                ? new BSediment
                {
                    Id = dto.BSediment.Id == Guid.Empty ? Guid.NewGuid() : dto.BSediment.Id,
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
                    Id = dto.BSensorisk.Id == Guid.Empty ? Guid.NewGuid() : dto.BSensorisk.Id,
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
                    Id = dto.BAnimal.Id == Guid.Empty ? Guid.NewGuid() : dto.BAnimal.Id,
                    Pigghunder = dto.BAnimal.Pigghunder,
                    Krepsdyr = dto.BAnimal.Krepsdyr,
                    Skjell = dto.BAnimal.Skjell,
                    Borstemark = dto.BAnimal.Borstemark,
                    Arter = dto.BAnimal.Arter
                }
                : null,
        };
    }
    
        public static void ApplyEditSurveyDto(this BUndersokelse entity, EditSurveyDto dto)
    {
        if (entity == null || dto == null) return;

        entity.ProsjektId = dto.ProsjektId;
        entity.PreinfoId = dto.PreinfoId;
        entity.Feltdato = dto.Feltdato;
        entity.AntallGrabbhugg = dto.AntallGrabbhugg;
        entity.GrabbhastighetGodkjent = dto.GrabbhastighetGodkjent;
        entity.Beggiatoa = dto.Beggiatoa;
        entity.Forrester = dto.Forrester;
        entity.Fekalier = dto.Fekalier;
        entity.Merknader = dto.Merknader;
        entity.DatoEndret = dto.DatoEndret;
        entity.IndeksGr2Gr3 = dto.IndeksGr2Gr3;
        entity.TilstandGr2Gr3 = dto.TilstandGr2Gr3;
        entity.Korrigeringer = dto.Korrigeringer ?? "";

        if (entity.BStasjon != null && dto.BStation != null)
        {
            entity.BStasjon.KoordinatNord = dto.BStation.KoordinatNord;
            entity.BStasjon.KoordinatOst = dto.BStation.KoordinatOst;
            entity.BStasjon.Dybde = dto.BStation.Dybde;
            entity.BStasjon.Analyser = dto.BStation.Analyser;
        }

        if (entity.Blotbunn != null && dto.BSoftBase != null)
        {
            entity.Blotbunn.Leire = dto.BSoftBase.Leire;
            entity.Blotbunn.Silt = dto.BSoftBase.Silt;
            entity.Blotbunn.Sand = dto.BSoftBase.Sand;
            entity.Blotbunn.Grus = dto.BSoftBase.Grus;
            entity.Blotbunn.Skjellsand = dto.BSoftBase.Skjellsand;
        }
        else if (entity.Blotbunn == null && dto.BSoftBase != null)
        {
            entity.HardbunnId = null;
            entity.Hardbunn = null;
            
            entity.Blotbunn = new BBlotbunn
            {
                Id = dto.BlotbunnId ?? dto.BSoftBase.Id,
                Leire = dto.BSoftBase.Leire,
                Silt = dto.BSoftBase.Silt,
                Sand = dto.BSoftBase.Sand,
                Grus = dto.BSoftBase.Grus,
                Skjellsand = dto.BSoftBase.Skjellsand
            };
        }
        else if (entity.Blotbunn != null && dto.BSoftBase == null)
        {
            entity.BlotbunnId = null;
            entity.Blotbunn = null;
        }

        if (entity.Hardbunn != null && dto.BHardBase != null)
        {
            entity.Hardbunn.Steinbunn = dto.BHardBase.Steinbunn;
            entity.Hardbunn.Fjellbunn = dto.BHardBase.Fjellbunn;
        }
        else if (entity.Hardbunn == null && dto.BHardBase != null)
        {
            entity.BlotbunnId = null;
            entity.Blotbunn = null;
            
            entity.Hardbunn = new BHardbunn
            {
                Id = dto.HardbunnId ?? dto.BHardBase.Id,
                Steinbunn = dto.BHardBase.Steinbunn,
                Fjellbunn = dto.BHardBase.Fjellbunn
            };
        }
        else if (entity.Hardbunn != null && dto.BHardBase == null)
        {
            entity.HardbunnId = null;
            entity.Hardbunn = null;
        }

        if (entity.Sediment != null && dto.BSediment != null)
        {
            entity.Sediment.Ph = dto.BSediment.Ph;
            entity.Sediment.Eh = dto.BSediment.Eh;
            entity.Sediment.Temperatur = dto.BSediment.Temperatur;
            entity.Sediment.KlasseGr2 = dto.BSediment.KlasseGr2;
            entity.Sediment.TilstandGr2 = dto.BSediment.TilstandGr2;
        }
        else if (entity.Sediment == null && dto.BSediment != null)
        {
            entity.Sediment = new BSediment
            {
                Id = dto.SedimentId ?? dto.BSediment.Id,
                Ph = dto.BSediment.Ph,
                Eh = dto.BSediment.Eh,
                Temperatur = dto.BSediment.Temperatur,
                KlasseGr2 = dto.BSediment.KlasseGr2,
                TilstandGr2 = dto.BSediment.TilstandGr2
            };
            entity.HardbunnId = null;
            entity.Hardbunn = null;
            entity.BlotbunnId = dto.BlotbunnId;
        }
        else if (entity.Sediment != null && dto.BSediment == null)
        {
            entity.SedimentId = null;
            entity.Sediment = null;

            entity.BlotbunnId = null;
            entity.Blotbunn = null;
            entity.HardbunnId = dto.HardbunnId;
        }

        if (entity.Sensorisk != null && dto.BSensorisk != null)
        {
            entity.Sensorisk.Gassbobler = dto.BSensorisk.Gassbobler;
            entity.Sensorisk.Farge = dto.BSensorisk.Farge;
            entity.Sensorisk.Lukt = dto.BSensorisk.Lukt;
            entity.Sensorisk.Konsistens = dto.BSensorisk.Konsistens;
            entity.Sensorisk.Grabbvolum = dto.BSensorisk.Grabbvolum;
            entity.Sensorisk.Tykkelseslamlag = dto.BSensorisk.Tykkelseslamlag;
            entity.Sensorisk.IndeksGr3 = dto.BSensorisk.IndeksGr3;
            entity.Sensorisk.TilstandGr3 = dto.BSensorisk.TilstandGr3;
        }
        else if (entity.Sensorisk == null && dto.BSensorisk != null)
        {
            entity.Sensorisk = new BSensorisk
            {
                Id = dto.SensoriskId ?? dto.BSensorisk.Id,
                Gassbobler = dto.BSensorisk.Gassbobler,
                Farge = dto.BSensorisk.Farge,
                Lukt = dto.BSensorisk.Lukt,
                Konsistens = dto.BSensorisk.Konsistens,
                Grabbvolum = dto.BSensorisk.Grabbvolum,
                Tykkelseslamlag = dto.BSensorisk.Tykkelseslamlag,
                IndeksGr3 = dto.BSensorisk.IndeksGr3,
                TilstandGr3 = dto.BSensorisk.TilstandGr3
            };
        }
        else if (entity.Sensorisk != null && dto.BSensorisk == null)
        {
            entity.SedimentId = null;
            entity.Sediment = null;
            entity.SensoriskId = null;
            entity.Sensorisk = null;
            entity.BlotbunnId = null;
            entity.Blotbunn = null;
            entity.HardbunnId = dto.HardbunnId;
        }

        if (entity.Dyr != null && dto.BAnimal != null)
        {
            entity.Dyr.Pigghunder = dto.BAnimal.Pigghunder;
            entity.Dyr.Krepsdyr = dto.BAnimal.Krepsdyr;
            entity.Dyr.Skjell = dto.BAnimal.Skjell;
            entity.Dyr.Borstemark = dto.BAnimal.Borstemark;
            entity.Dyr.Arter = dto.BAnimal.Arter;
        }
        else if (entity.Dyr == null && dto.BAnimal != null)
        {
            entity.Dyr = new BDyr
            {
                Id = dto.DyrId ?? dto.BAnimal.Id,
                Pigghunder = dto.BAnimal.Pigghunder,
                Krepsdyr = dto.BAnimal.Krepsdyr,
                Skjell = dto.BAnimal.Skjell,
                Borstemark = dto.BAnimal.Borstemark,
                Arter = dto.BAnimal.Arter
            };
        }
        else if (entity.Dyr != null && dto.BAnimal == null)
        {
            entity.DyrId = null;
            entity.Dyr = null;
        }
    }
}
