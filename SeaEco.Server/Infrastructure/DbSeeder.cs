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
                Produksjonsstatus = 0,
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
                Produksjonsstatus = 2,
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
                Feltdato = DateTime.Now,
                FeltansvarligId = feltansvarligId
            },
            new BPreinfo()
            {
                Id = preinfoId2,
                ProsjektId = prosjektId2,
                Feltdato = DateTime.Now,
                FeltansvarligId = feltansvarligId
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
    }
}