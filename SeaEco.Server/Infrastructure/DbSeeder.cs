using Microsoft.EntityFrameworkCore;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;
using SeaEco.Services.HashService;

namespace SeaEco.Server.Infrastructure;

public sealed class DbSeeder
{
    public async Task SeedData(AppDbContext context)
    {
        // Seed Bruker
        Guid adminId = Guid.Parse("8fffdaa4-7dfe-4d78-a28b-b80558d542b6");
        
        Bruker? admin = context.Brukers.FirstOrDefault(x => x.Id == adminId);
        if (admin is null)
        {
            var password = Hasher.Hash("1111");
            admin = new()
            {   
                Id = adminId,
                Fornavn = "admin",
                Etternavn = "admin",
                Epost = "gruppe202520@gmail.com",
                PassordHash = password.hashed,
                Salt = password.salt,
                IsAdmin = true,
                Aktiv = true
            };
        
            await context.Brukers.AddAsync(admin);
            await context.SaveChangesAsync();
        }

        // Seed Lokalitet
        Guid lokalitetId1 = Guid.Parse("546e9ab0-9bbb-41be-95be-af879924b192");
        Guid lokalitetId2 = Guid.Parse("c8fe96cd-78a7-4e97-901e-965d2c48e113");
        Guid lokalitetId3 = Guid.Parse("2b79465d-228a-40eb-ae7e-2db7e3f672e7");

        IEnumerable<Lokalitet> lokalitets =
        [
            new Lokalitet()
            {
                Id = lokalitetId1,
                Lokalitetsnavn = "Myrlandshaug",
                LokalitetsId = "11332"
            },
            new Lokalitet()
            {
                Id = lokalitetId2,
                Lokalitetsnavn = "Bergvikodden",
                LokalitetsId = "11241"
            },
            new Lokalitet()
            {
                Id = lokalitetId3,
                Lokalitetsnavn = "Dragnes",
                LokalitetsId = "10505"
            }
        ];

        List<Lokalitet> lokalitetRecords = await context.Lokalitets.Where(x => 
            x.Id == lokalitetId1 ||
            x.Id == lokalitetId2 ||
            x.Id == lokalitetId3).ToListAsync();
        
        foreach (Lokalitet l in lokalitets)
        {
            if (!lokalitetRecords.Any(x => x.Id == l.Id))
            {
                await context.Lokalitets.AddAsync(l);
                await context.SaveChangesAsync();
            }
        }
        
        // Seed Kunder 
        Guid kundeId1 = Guid.Parse("0f7a3a3e-55c9-4317-93eb-ed8b5741e04e");
        Guid kundeId2 = Guid.Parse("7b4fc6a6-4a5b-47f7-9ca0-58de6ad622e4");
        Guid kundeId3 = Guid.Parse("80cd9f7e-dd53-40b9-9e3a-f57b31819a97");

        IEnumerable<Kunde> kunders =
        [
            new Kunde()
            {
                Id = kundeId1,
                Oppdragsgiver = "Gratanglaks AS",
                Kontaktperson = "Marius Hagen",
                Telefon = "11111111",
            },
            new Kunde()
            {
                Id = kundeId2,
                Oppdragsgiver = "Seafood AS",
                Kontaktperson = "Andreas Dahl",
                Telefon = "22222222",
            },
            new Kunde()
            {
                Id = kundeId3,
                Oppdragsgiver = "Aalesundfisk AS",
                Kontaktperson = "Ola Pedersen",
                Telefon = "33333333",
            }
        ];
        
        List<Kunde> kundeRecords = await context.Kundes.Where(x => 
            x.Id == kundeId1 ||
            x.Id == kundeId2 ||
            x.Id == kundeId3).ToListAsync();
        
        foreach (Kunde l in kunders)
        {
            if (!kundeRecords.Any(x => x.Id == l.Id))
            {
                await context.Kundes.AddAsync(l);
                await context.SaveChangesAsync();
            }
        }
        
        //Seed Bprosjekt 
        Guid prosjektId1 = Guid.Parse("b53dedcd-481c-4f00-b537-7304d6093d7d"); 
        Guid prosjektId2 = Guid.Parse("6f44505b-b08e-4418-ae2d-8d75a6f35131");
        
        IEnumerable<BProsjekt> prosjekts =
        [
            new BProsjekt()
            {
                Id = prosjektId1,
                Datoregistrert = DateTime.Now,
                Kundeepost = "test@seaeco.no",
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Merknad = "Kommentar",
                Prosjektstatus = 0,
                Mtbtillatelse = 100,
                Produksjonsstatus = 2,
                KundeId = kundeId1,
                LokalitetId = lokalitetId1,
                PoId = "406",
                ProsjektIdSe = "SE-25-BU-1",
                ProsjektansvarligId = Guid.Parse("8fffdaa4-7dfe-4d78-a28b-b80558d542b6")
            },
            new BProsjekt()
            {
                Id = prosjektId2,
                Datoregistrert = DateTime.Now,
                Kundeepost = "test@test.no",
                Kundekontaktperson = "Ole Kristiansen",
                Kundetlf = "11111111",
                Merknad = "Kommentar",
                Prosjektstatus = 0,
                Mtbtillatelse = 200,
                Produksjonsstatus = 1,
                KundeId = kundeId2,
                LokalitetId = lokalitetId2,
                PoId = "407",
                ProsjektIdSe = "SE-25-BU-2",
                ProsjektansvarligId = Guid.Parse("8fffdaa4-7dfe-4d78-a28b-b80558d542b6")
            }
        ];
        
        List<BProsjekt> prosjektRecords = await context.BProsjekts.Where(x => 
            x.Id == prosjektId1 ||
            x.Id == prosjektId2).ToListAsync();
        
        foreach (BProsjekt l in prosjekts)
        {
            if (!prosjektRecords.Any(x => x.Id == l.Id))
            {
                await context.BProsjekts.AddAsync(l);
                await context.SaveChangesAsync();
            }
        }
        
        // Seed BPreinfo
        Guid preinfoId1 = Guid.Parse("cc2642ba-0cd4-49a6-92cb-cb7eb2f15e7d");
        Guid preinfoId2 = Guid.Parse("a7ad11e6-edb4-40bf-b7af-51fefb215788");


        Guid feltansvarligId = Guid.Parse("8fffdaa4-7dfe-4d78-a28b-b80558d542b6");

        IEnumerable<BPreinfo> preinfos =
        [
            new BPreinfo()
            {
                Id = preinfoId1,
                ProsjektId = prosjektId1,
                Feltdato = new DateTime(2025, 4, 8, 8, 30, 0),
                FeltansvarligId = feltansvarligId,
                PhSjo = 7.94f,
                EhSjo = 185.2f,
                SjoTemperatur = 10.8f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 4, 7)
            },
            new BPreinfo()
            {
                Id = preinfoId2,
                ProsjektId = prosjektId2,
                Feltdato = new DateTime(2025, 4, 25, 10, 0, 0),
                FeltansvarligId = feltansvarligId,
                PhSjo = 6.94f,
                EhSjo = 182.0f,
                SjoTemperatur = 11.1f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 4, 24)
            }
        ];

        List<BPreinfo> preinfoRecords = await context.BPreinfos.Where(x =>
            x.Id == preinfoId1 ||
            x.Id == preinfoId2).ToListAsync();

        foreach (BPreinfo p in preinfos)
        {
            if (!preinfoRecords.Any(x => x.Id == p.Id))
            {
                await context.BPreinfos.AddAsync(p);
                await context.SaveChangesAsync();
            }
        }
        
        // Seed BUndersokelse
        Guid undersokelseId1 = Guid.Parse("3f7e477f-a26e-4b5b-93f3-546b6be693d1");
        Guid undersokelseId2 = Guid.Parse("6aa7a8eb-1491-4ebe-9901-09553a9f71f8");

        IEnumerable<BUndersokelse> undersokelser =
        [
            new BUndersokelse()
            {
                Id = undersokelseId1,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId1,
                Feltdato = DateOnly.FromDateTime(DateTime.Now),
                AntallGrabbhugg = 5,
                GrabbhastighetGodkjent = true,
                Beggiatoa = false,
                Forrester = true,
                Fekalier = false,
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now
            },
            new BUndersokelse()
            {
                Id = undersokelseId2,
                ProsjektId = prosjektId2,
                PreinfoId = preinfoId2,
                Feltdato = DateOnly.FromDateTime(DateTime.Now),
                AntallGrabbhugg = 8,
                GrabbhastighetGodkjent = true,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = false,
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now
            }
        ];

        List<BUndersokelse> undersokelseRecords = await context.BUndersokelses.Where(x =>
            x.Id == undersokelseId1 ||
            x.Id == undersokelseId2).ToListAsync();

        foreach (BUndersokelse u in undersokelser)
        {
            if (!undersokelseRecords.Any(x => x.Id == u.Id))
            {
                await context.BUndersokelses.AddAsync(u);
                await context.SaveChangesAsync();
            }
        }
        
                
        // Seed BStasjon
        Guid stasjonId1 = Guid.Parse("03db101b-581b-46da-8bc8-726c1d9d31aa");

        List<BStasjon> stasjoner =
        [
            new BStasjon
            {
                Id = stasjonId1,
                ProsjektId = prosjektId1,
                Nummer = 11,
                KoordinatNord = "68°46.851",
                KoordinatOst = "17°17.036",
                Dybde = 105,
                Analyser = "B",
                ProvetakingsplanId = null, 
                UndersokelseId = undersokelseId1
            }
        ];

        List<BStasjon> stasjonRecords = await context.BStasjons.Where(x => x.Id == stasjonId1).ToListAsync();
        foreach (BStasjon s in stasjoner)
        {
            if (!stasjonRecords.Any(x => x.Id == s.Id))
            {
                await context.BStasjons.AddAsync(s);
                await context.SaveChangesAsync();
            }
        }

        // Seed BBlotbunn
        Guid blotbunnId1 = Guid.Parse("830eebc8-6d86-40ad-88ed-3cbe3e43e0bc");
        var blotbunn = new BBlotbunn
        {
            Id = blotbunnId1,
            Leire = 0,
            Silt = 1,
            Sand = 0,
            Grus = 1,
            Skjellsand = 0
        };
        if (!await context.BBlotbunns.AnyAsync(x => x.Id == blotbunnId1))
        {
            await context.BBlotbunns.AddAsync(blotbunn);
            await context.SaveChangesAsync();
        }

        // Seed BHardbunn
        Guid hardbunnId1 = Guid.Parse("51042d7a-4b05-425c-a4b7-f373ba21a0be");
        var hardbunn = new BHardbunn
        {
            Id = hardbunnId1,
            Steinbunn = 0,
            Fjellbunn = 1
        };
        if (!await context.BHardbunns.AnyAsync(x => x.Id == hardbunnId1))
        {
            await context.BHardbunns.AddAsync(hardbunn);
            await context.SaveChangesAsync();
        }

        // Seed BSensorisk
        Guid sensoriskId1 = Guid.Parse("02843db6-6a65-4f45-85c2-4537096686e6");
        var sensorisk = new BSensorisk
        {
            Id = sensoriskId1,
            Gassbobler = 1,
            Farge = 2,
            Lukt = 2,
            Konsistens = 2,
            Grabbvolum = 5,
            Tykkelseslamlag = 3,
            IndeksGr3 = 0.5f,
            TilstandGr3 = 2
        };
        if (!await context.BSensorisks.AnyAsync(x => x.Id == sensoriskId1))
        {
            await context.BSensorisks.AddAsync(sensorisk);
            await context.SaveChangesAsync();
        }

        // Seed BDyr
        Guid dyrId1 = Guid.Parse("ffcf1a63-5530-4163-a38b-b34df2406979");
        var dyr = new BDyr
        {
            Id = dyrId1,
            Pigghunder = "1",
            Krepsdyr = "3",
            Skjell = "50+",
            Borstemark = "50+",
            Arter = "Andre arter: 2"
        };
        if (!await context.BDyrs.AnyAsync(x => x.Id == dyrId1))
        {
            await context.BDyrs.AddAsync(dyr);
            await context.SaveChangesAsync();
        }

        // Обновление BUndersokelse связями
        var undersokelse = await context.BUndersokelses.FirstOrDefaultAsync(x => x.Id == undersokelseId1);
        if (undersokelse != null)
        {
            undersokelse.BlotbunnId = blotbunnId1;
            undersokelse.HardbunnId = hardbunnId1;
            undersokelse.SensoriskId = sensoriskId1;
            undersokelse.DyrId = dyrId1;
            undersokelse.Merknader = "Rester av anleggsmateriale";
            context.BUndersokelses.Update(undersokelse);
            await context.SaveChangesAsync();
        }

    }
}