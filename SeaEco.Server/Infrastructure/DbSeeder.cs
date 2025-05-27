using Microsoft.EntityFrameworkCore;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Services.HashService;

namespace SeaEco.Server.Infrastructure;

public sealed class DbSeeder
{
    public async Task SeedData(AppDbContext context, bool regenerate)
    {
        if (regenerate)
        {
            context.TruncateAllTablesPostgres();
        }
        
        // ProgramVersjon
        Guid programversjonId1 = Guid.Parse("6a1c5580-6594-4e46-9149-e85c80186a1a");
        Guid programversjonId2 = Guid.Parse("73a93d1e-375f-454a-b48d-fe85e6bd0f5d");
        Guid programversjonId3 = Guid.Parse("d6653d1c-017f-47fa-b0da-f4f7024f0e7e");

        List<Programversjon> programversjons =
        [
            new Programversjon
            {
                Id = programversjonId1,
                Utgivelsesdato = new DateOnly(2025, 3, 21),
                Versjonsnummer = "0.1-beta",
                Forbedringer = @"
                - Første betautgivelse av applikasjonen
                - Implementert grunnleggende navigasjon mellom seksjoner
                - Lagt til påloggings-/registreringsside
                ".Trim()
            },
            new Programversjon
            {
                Id = programversjonId2,
                Utgivelsesdato = new DateOnly(2025, 3, 30),
                Versjonsnummer = "0.2-beta",
                Forbedringer = @"
                - Integrasjon med database via EF Core
                - Lagt til brukerprofilside
                - Forbedret validering av skjemaer (passord, e-post)                
                ".Trim()
            },
            new Programversjon
            {
                Id = programversjonId3,
                Utgivelsesdato = new DateOnly(2025, 4, 1),
                Versjonsnummer = "0.3-beta",
                Forbedringer = @"
                - Rettet kritiske feil i autorisasjonsmodulen
                ".Trim()
            }
        ];

        await context.Programversjons.AddRangeAsync(programversjons);
        await context.SaveChangesAsync();
        
        // Seed Bruker
        Guid adminId1 = Guid.Parse("8fffdaa4-7dfe-4d78-a28b-b80558d542b6");
        Guid adminId2 = Guid.Parse("88439bb4-440d-4b5c-9d2b-bd6c0b3b68ff");
        Guid adminId3 = Guid.Parse("73510705-b951-4753-9cfc-93d903517df0");
        Guid brukerId2 = Guid.Parse("e7c66db8-d146-4802-90c2-a4e18b14c577");
        Guid brukerId3 = Guid.Parse("e44fe6d6-92f0-4d9e-a599-daa3eff47f70");
        Guid brukerId4 = Guid.Parse("b427f17b-f150-4b7a-b53f-bf2529ad713e");
        
        var password1 = Hasher.Hash("11111111");
        var password2 = Hasher.Hash("11111111");
        var password3 = Hasher.Hash("11111111");
        var password5 = Hasher.Hash("11111111");
        var password6 = Hasher.Hash("11111111");
        var password7 = Hasher.Hash("11111111");
        
        IEnumerable<Bruker> users =
        [
            new Bruker()
            {   
                Id = adminId1,
                Fornavn = "Admin",
                Etternavn = "Main",
                Epost = "gruppe202520@gmail.com",
                PassordHash = password1.hashed,
                Salt = password1.salt,
                IsAdmin = true,
                Aktiv = true
            },
            new Bruker()
            {   
                Id = adminId2,
                Fornavn = "Adel",
                Etternavn = "Pedersen",
                Epost = "admin2@test.no",
                PassordHash = password2.hashed,
                Salt = password2.salt,
                IsAdmin = true,
                Aktiv = true
            },
            new Bruker()
            {   
                Id = adminId3,
                Fornavn = "Amelie",
                Etternavn = "Hansen",
                Epost = "admin3@test.no",
                PassordHash = password3.hashed,
                Salt = password3.salt,
                IsAdmin = true,
                Aktiv = true
            },
            new Bruker()
            {   
                Id = brukerId2,
                Fornavn = "Ola",
                Etternavn = "Solberg",
                Epost = "bruker2@test.no",
                PassordHash = password5.hashed,
                Salt = password5.salt,
                IsAdmin = false,
                Aktiv = false
            },
            new Bruker()
            {   
                Id = brukerId3,
                Fornavn = "Ingrid",
                Etternavn = "Mikkelsen",
                Epost = "bruker3@test.no",
                PassordHash = password6.hashed,
                Salt = password6.salt,
                IsAdmin = false,
                Aktiv = true
            },
            new Bruker()
            {   
                Id = brukerId4,
                Fornavn = "Lars",
                Etternavn = "Nilsen",
                Epost = "bruker4@test.no",
                PassordHash = password7.hashed,
                Salt = password7.salt,
                IsAdmin = false,
                Aktiv = false
            },
        ];
        
        await context.Brukers.AddRangeAsync(users);
        await context.SaveChangesAsync();

        // Seed Lokalitet
        Guid lokalitetId1 = Guid.Parse("546e9ab0-9bbb-41be-95be-af879924b192");
        Guid lokalitetId2 = Guid.Parse("c8fe96cd-78a7-4e97-901e-965d2c48e113");
        Guid lokalitetId3 = Guid.Parse("2b79465d-228a-40eb-ae7e-2db7e3f672e7");
        Guid lokalitetId4 = Guid.Parse("a8a420c2-6b34-4764-adeb-47dd5e26108c");
        Guid lokalitetId5 = Guid.Parse("ab55532b-e5dd-4fb2-a171-eecd5ab8ec5f");
        Guid lokalitetId6 = Guid.Parse("42ccaaa7-5075-4245-88ea-3d10809e335e");
        Guid lokalitetId7 = Guid.Parse("b80913f9-2c45-49ca-bfb7-5737857e74d0");
        Guid lokalitetId8 = Guid.Parse("e1997a7d-da30-4aac-8e7c-3bfa1ac5d1c1");

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
            },
            new Lokalitet()
            {
                Id = lokalitetId4,
                Lokalitetsnavn = "Skjervøy",
                LokalitetsId = "10456"
            },
            new Lokalitet()
            {
                Id = lokalitetId5,
                Lokalitetsnavn = "Rekvik",
                LokalitetsId = "10789"
            },
            new Lokalitet()
            {
                Id = lokalitetId6,
                Lokalitetsnavn = "Kvaløysletta",
                LokalitetsId = "10934"
            },
            new Lokalitet()
            {
                Id = lokalitetId7,
                Lokalitetsnavn = "Husøy",
                LokalitetsId = "11123"
            },
            new Lokalitet()
            {
                Id = lokalitetId8,
                Lokalitetsnavn = "Torsken",
                LokalitetsId = "11567"
            }
        ];
        
        await context.Lokalitets.AddRangeAsync(lokalitets);
        await context.SaveChangesAsync();
        
        
        // Seed Kunder 
        Guid kundeId1 = Guid.Parse("0f7a3a3e-55c9-4317-93eb-ed8b5741e04e");
        Guid kundeId2 = Guid.Parse("7b4fc6a6-4a5b-47f7-9ca0-58de6ad622e4");
        Guid kundeId3 = Guid.Parse("80cd9f7e-dd53-40b9-9e3a-f57b31819a97");
        Guid kundeId4 = Guid.Parse("69357ab2-7c39-4844-a071-822707e5c386");
        Guid kundeId5 = Guid.Parse("13d35c73-f635-44a5-99ee-421730294dd7");
        Guid kundeId6 = Guid.Parse("16826f0b-928a-4398-bf38-d476189c06c9");
        Guid kundeId7 = Guid.Parse("0541770a-ae40-49cc-99bf-e32bf466566f");
        Guid kundeId8 = Guid.Parse("3b504ba6-926a-491f-9ab2-2c4d7d4345c5");
        Guid kundeId9 = Guid.Parse("b0a13185-8a83-498e-82d8-f2979f174adf");

        IEnumerable<Kunde> kundes =
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
            },
            new Kunde()
            {
                Id = kundeId4,
                Oppdragsgiver = "Bergen Marine AS",
                Kontaktperson = "Kjetil Haugland",
                Telefon = "44444444",
            },
            new Kunde()
            {
                Id = kundeId5,
                Oppdragsgiver = "Nordhavet Oppdrett AS",
                Kontaktperson = "Lene Øverås",
                Telefon = "55555555",
            },
            new Kunde()
            {
                Id = kundeId6,
                Oppdragsgiver = "Fjordfisk AS",
                Kontaktperson = "Terje Karlsen",
                Telefon = "66666666",
            },
            new Kunde()
            {
                Id = kundeId7,
                Oppdragsgiver = "Vesterhavs Laks AS",
                Kontaktperson = "Silje Mikkelsen",
                Telefon = "77777777",
            },
            new Kunde()
            {
                Id = kundeId8,
                Oppdragsgiver = "Havbruk Nord AS",
                Kontaktperson = "Rune Bø",
                Telefon = "88888888",
            },
            new Kunde()
            {
                Id = kundeId9,
                Oppdragsgiver = "Kystoppdrett AS",
                Kontaktperson = "Ida Nilsen",
                Telefon = "99999999",
            }
        ];
        
        await context.Kundes.AddRangeAsync(kundes);
        await context.SaveChangesAsync();
        
        
        //Seed Bprosjekt 
        Guid prosjektId1 = Guid.Parse("b53dedcd-481c-4f00-b537-7304d6093d7d"); 
        Guid prosjektId2 = Guid.Parse("6f44505b-b08e-4418-ae2d-8d75a6f35131");

        Guid prosjektId3 = Guid.Parse("f382b818-bf41-4432-9906-dff6391518a5");
        Guid prosjektId4 = Guid.Parse("27926a13-ebbe-458c-bfc3-7935525ed270");
        Guid prosjektId5 = Guid.Parse("64a1e81d-9281-48a5-a791-95f08a9d0525");
        
        
        Guid prosjektId6 = Guid.Parse("41989256-9a6a-46b1-b575-23ed3d9407f8");
        Guid prosjektId7 = Guid.Parse("83d2d8cb-e3c5-4563-a1c4-68b8aad3685b");
        Guid prosjektId8 = Guid.Parse("d7838590-b4db-49b8-b410-1e1a83696bf6");
        
        Guid prosjektId9 = Guid.Parse("5bcb9c1e-4b2b-4a1c-8f58-cb54be2a301d");
        Guid prosjektId10 = Guid.Parse("7c3e95ad-9ce2-4349-8cf7-69097eddf0bc");
        Guid prosjektId11 = Guid.Parse("d8e75a4b-2c08-4bc7-9958-2779a81ceb12");


        
        IEnumerable<BProsjekt> prosjekts =
        [
            new BProsjekt()
            {
                Id = prosjektId1,
                ProsjektIdSe = "SE-25-BU-1",
                PoId = "406",
                KundeId = kundeId1,
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Kundeepost = "test@seaeco.no",
                LokalitetId = lokalitetId1,
                Mtbtillatelse = 100,
                ProsjektansvarligId = adminId1,
                Produksjonsstatus = 2,
                Merknad = "Kommentar",
                Prosjektstatus = 4,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId2,
                ProsjektIdSe = "SE-25-BU-2",
                PoId = "407",
                KundeId = kundeId2,
                Kundekontaktperson = "Ole Kristiansen",
                Kundetlf = "11111111",
                Kundeepost = "test@test.no",
                LokalitetId = lokalitetId2,
                Mtbtillatelse = 200,
                ProsjektansvarligId = adminId2,
                Produksjonsstatus = 1,
                Merknad = "Kommentar",
                Prosjektstatus = 3,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId3,
                ProsjektIdSe = null,
                PoId = "408",
                KundeId = kundeId1,
                Kundekontaktperson = "Ole Kristiansen",
                Kundetlf = "11111111",
                Kundeepost = "test@test.no",
                LokalitetId = lokalitetId4,
                Mtbtillatelse = 210,
                ProsjektansvarligId = adminId3,
                Produksjonsstatus = 2,
                Merknad = "Kommentar",
                Prosjektstatus = 2,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId4,
                ProsjektIdSe = "SE-25-BU-3",
                PoId = "409",
                KundeId = kundeId1,
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Kundeepost = "test@seaeco.no",
                LokalitetId = lokalitetId4,
                Mtbtillatelse = 150,
                ProsjektansvarligId = adminId1,
                Produksjonsstatus = 3,
                Merknad = "Kommentar",
                Prosjektstatus = 4,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId5,
                ProsjektIdSe = null,
                PoId = "410",
                KundeId = kundeId2,
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Kundeepost = "test@seaeco.no",
                LokalitetId = lokalitetId6,
                Mtbtillatelse = 180,
                ProsjektansvarligId = adminId3,
                Produksjonsstatus = 4,
                Merknad = "Kommentar",
                Prosjektstatus = 5,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId6,
                ProsjektIdSe = "SE-25-BU-4",
                PoId = "411",
                KundeId = kundeId4,
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Kundeepost = "test@seaeco.no",
                LokalitetId = lokalitetId6,
                Mtbtillatelse = 180,
                ProsjektansvarligId = adminId3,
                Produksjonsstatus = 2,
                Merknad = "Kommentar",
                Prosjektstatus = 4,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId7,
                ProsjektIdSe = "SE-25-BU-5",
                PoId = "412",
                KundeId = kundeId6,
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Kundeepost = "test@seaeco.no",
                LokalitetId = lokalitetId6,
                Mtbtillatelse = 180,
                ProsjektansvarligId = adminId1,
                Produksjonsstatus = 1,
                Merknad = "Kommentar",
                Prosjektstatus = 4,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId8,
                ProsjektIdSe = "SE-25-BU-6",
                PoId = "413",
                KundeId = kundeId9,
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Kundeepost = "test@seaeco.no",
                LokalitetId = lokalitetId6,
                Mtbtillatelse = 180,
                ProsjektansvarligId = adminId2,
                Produksjonsstatus = 4,
                Merknad = "Kommentar",
                Prosjektstatus = 4,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId9,
                ProsjektIdSe = null,
                PoId = "414",
                KundeId = kundeId5,
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Kundeepost = "test@seaeco.no",
                LokalitetId = lokalitetId5,
                Mtbtillatelse = 180,
                ProsjektansvarligId = adminId1,
                Produksjonsstatus = 3,
                Merknad = "Kommentar",
                Prosjektstatus = 1,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId10,
                ProsjektIdSe = null,
                PoId = "415",
                KundeId = kundeId3,
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Kundeepost = "test@seaeco.no",
                LokalitetId = lokalitetId3,
                Mtbtillatelse = 180,
                ProsjektansvarligId = adminId2,
                Produksjonsstatus = 1,
                Merknad = "Kommentar",
                Prosjektstatus = 1,
                Datoregistrert = DateTime.Now
            },
            new BProsjekt()
            {
                Id = prosjektId11,
                ProsjektIdSe = null,
                PoId = "416",
                KundeId = kundeId7,
                Kundekontaktperson = "Lars Johansen",
                Kundetlf = "11111111",
                Kundeepost = "test@seaeco.no",
                LokalitetId = lokalitetId7,
                Mtbtillatelse = 180,
                ProsjektansvarligId = adminId3,
                Produksjonsstatus = 4,
                Merknad = "Kommentar",
                Prosjektstatus = 5,
                Datoregistrert = DateTime.Now
            }
        ];
        
        await context.BProsjekts.AddRangeAsync(prosjekts);
        await context.SaveChangesAsync();
        
        //Seed BProvetakingsplan
        Guid provetakingsplanId1 = Guid.Parse("82ca018f-f197-42ae-832a-01fdc895f562");
        Guid provetakingsplanId2 = Guid.Parse("4b418d62-958f-4e66-ba13-fa24311c43b0"); 
        Guid provetakingsplanId3 = Guid.Parse("48a58452-1428-4dc9-9697-f4374cf8d226"); 
        Guid provetakingsplanId4 = Guid.Parse("1576ef68-8428-4d12-99d1-eb8bd2c35153"); 

        
        List<BProvetakningsplan> provetakningsplans =
        [
            new BProvetakningsplan
            {
                Id = provetakingsplanId1,
                ProsjektId = prosjektId1,
                Planlagtfeltdato = new DateOnly(2025, 3, 30),
                PlanleggerId = adminId1 
            },
            new BProvetakningsplan
            {
                Id = provetakingsplanId2,
                ProsjektId = prosjektId2,
                Planlagtfeltdato = new DateOnly(2025, 4, 5),
                PlanleggerId = adminId2 
            },
            new BProvetakningsplan
            {
                Id = provetakingsplanId3,
                ProsjektId = prosjektId3,
                Planlagtfeltdato = new DateOnly(2025, 4, 1),
                PlanleggerId = adminId3 
            },
            new BProvetakningsplan
            {
                Id = provetakingsplanId4,
                ProsjektId = prosjektId4,
                Planlagtfeltdato = new DateOnly(2025, 4, 10),
                PlanleggerId = brukerId2
            },
        ];
        
        await context.BProvetakningsplans.AddRangeAsync(provetakningsplans);
        await context.SaveChangesAsync();
        
        
        // Seed BPreinfo
        Guid preinfoId1 = Guid.Parse("cc2642ba-0cd4-49a6-92cb-cb7eb2f15e7d");
        Guid preinfoId2 = Guid.Parse("a7ad11e6-edb4-40bf-b7af-51fefb215788");
        Guid preinfoId3 = Guid.Parse("3fc3210c-8e04-4058-a2c1-3dfed7a8f522");
        Guid preinfoId4 = Guid.Parse("054ec9b6-16d6-4423-bd44-2ee648bafd9c");
        Guid preinfoId5 = Guid.Parse("71a858c5-6662-41e5-a290-d3d4ee63073f");
        Guid preinfoId6 = Guid.Parse("4aabf6c0-6ab5-43a2-b8cf-3bbba724c276");
        
        Guid preinfoId7 = Guid.Parse("b449f38c-dc89-43cc-9342-623f37847df9");
        Guid preinfoId8 = Guid.Parse("31922256-ef18-440b-9319-3870e69de4ff");
        Guid preinfoId9 = Guid.Parse("d7846568-f2e3-4088-be37-fc9b46c2223d");

        IEnumerable<BPreinfo> preinfos =
        [
            new BPreinfo()
            {
                Id = preinfoId1,
                ProsjektId = prosjektId1,
                Feltdato = new DateTime(2025, 4, 8, 8, 30, 0),
                FeltansvarligId = adminId1,
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
                ProsjektId = prosjektId1,
                Feltdato = new DateTime(2025, 4, 25, 10, 0, 0),
                FeltansvarligId = adminId1,
                PhSjo = 6.94f,
                EhSjo = 182.0f,
                SjoTemperatur = 11.1f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 4, 24)
            },
            new BPreinfo()
            {
                Id = preinfoId3,
                ProsjektId = prosjektId2,
                Feltdato = new DateTime(2025, 4, 28, 10, 0, 0),
                FeltansvarligId = adminId2,
                PhSjo = 6.94f,
                EhSjo = 182.0f,
                SjoTemperatur = 11.1f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 4, 24)
            },
            new BPreinfo()
            {
                Id = preinfoId4,
                ProsjektId = prosjektId2,
                Feltdato = new DateTime(2025, 5, 15, 10, 0, 0),
                FeltansvarligId = adminId2,
                PhSjo = 6.94f,
                EhSjo = 182.0f,
                SjoTemperatur = 11.1f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 5, 10)
            },
            new BPreinfo()
            {
                Id = preinfoId5,
                ProsjektId = prosjektId2,
                Feltdato = new DateTime(2025, 5, 1, 9, 0, 0),
                FeltansvarligId = adminId3,
                PhSjo = 6.94f,
                EhSjo = 182.0f,
                SjoTemperatur = 11.1f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 4, 30)
            },
            new BPreinfo()
            {
                Id = preinfoId6,
                ProsjektId = prosjektId4,
                Feltdato = new DateTime(2025, 5, 23, 9, 0, 0),
                FeltansvarligId = brukerId4,
                PhSjo = 6.94f,
                EhSjo = 182.0f,
                SjoTemperatur = 11.1f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 5, 22)
            },
            new BPreinfo()
            {
                Id = preinfoId7,
                ProsjektId = prosjektId6,
                Feltdato = new DateTime(2025, 5, 23, 9, 0, 0),
                FeltansvarligId = brukerId4,
                PhSjo = 6.94f,
                EhSjo = 182.0f,
                SjoTemperatur = 11.1f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 5, 22)
            },
            new BPreinfo()
            {
                Id = preinfoId8,
                ProsjektId = prosjektId7,
                Feltdato = new DateTime(2025, 5, 3, 9, 0, 0),
                FeltansvarligId = brukerId4,
                PhSjo = 6.94f,
                EhSjo = 182.0f,
                SjoTemperatur = 11.1f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 11, 22)
            },
            new BPreinfo()
            {
                Id = preinfoId9,
                ProsjektId = prosjektId8,
                Feltdato = new DateTime(2025, 5, 7, 9, 0, 0),
                FeltansvarligId = brukerId4,
                PhSjo = 6.94f,
                EhSjo = 182.0f,
                SjoTemperatur = 11.1f,
                RefElektrode = 0,
                Grabb = "2",
                Sil = "3",
                PhMeter = "2",
                Kalibreringsdato = new DateOnly(2025, 5, 22)
            },
        ];

        await context.BPreinfos.AddRangeAsync(preinfos);
        await context.SaveChangesAsync();
        
        
        // Seed BBlotbunn
        Guid blotbunnId1 = Guid.Parse("830eebc8-6d86-40ad-88ed-3cbe3e43e0bc");
        Guid blotbunnId2 = Guid.Parse("37e05b0b-0a82-45c3-a304-7e8426e1bf53");
        Guid blotbunnId3 = Guid.Parse("0d3724ca-3887-4bdc-b718-5bbe52f3e80d");
        Guid blotbunnId5 = Guid.Parse("a7c8c5d1-a62a-4391-b071-b0658a7eedda");
        
        Guid blotbunnId4 = Guid.Parse("2b9e8983-bbe9-46c0-bfbe-0fc83ba4d91c");
        Guid blotbunnId6 = Guid.Parse("5ffe5cda-883c-4f54-91e2-39ce09188070");
        Guid blotbunnId7 = Guid.Parse("a264ff96-2374-4b1c-92ca-7b4154526c1d");
        Guid blotbunnId8 = Guid.Parse("0b2f3e7b-87e6-4e70-b352-b7d106e79704");
        Guid blotbunnId9 = Guid.Parse("486b37b3-6793-45be-9618-61234c15b1c4");
        Guid blotbunnId10 = Guid.Parse("a195ff8a-dcb2-4f9f-8547-2d6b47ceee5e");

        
        List<BBlotbunn> blotbunns =
        [
            new BBlotbunn
            {
                Id = blotbunnId1,
                Leire = 0.5f,
                Silt = 1,
                Sand = 0,
                Grus = 1,
                Skjellsand = 0
            },
            new BBlotbunn
            {
                Id = blotbunnId2,
                Leire = 0,
                Silt = 1,
                Sand = 0.5f,
                Grus = 1,
                Skjellsand = 0
            },
            new BBlotbunn
            {
                Id = blotbunnId3,
                Leire = 0,
                Silt = 0.5f,
                Sand = 0,
                Grus = 1,
                Skjellsand = 0
            },
            new BBlotbunn
            {
                Id = blotbunnId5,
                Leire = 0,
                Silt = 0.5f,
                Sand = 0,
                Grus = 1,
                Skjellsand = 0
            },
            
            //new
            new BBlotbunn
            {
                Id = blotbunnId4,
                Leire = 0,
                Silt = 0.5f,
                Sand = 0,
                Grus = 1,
                Skjellsand = 0
            },
            new BBlotbunn
            {
                Id = blotbunnId6,
                Leire = 0,
                Silt = 0.5f,
                Sand = 0.5f,
                Grus = 1,
                Skjellsand = 0
            },
            new BBlotbunn
            {
                Id = blotbunnId7,
                Leire = 0,
                Silt = 1,
                Sand = 1,
                Grus = 0,
                Skjellsand = 0
            },
            new BBlotbunn
            {
                Id = blotbunnId8,
                Leire = 0.5f,
                Silt = 1,
                Sand = 0.5f,
                Grus = 0,
                Skjellsand = 0
            },
            new BBlotbunn
            {
                Id = blotbunnId9,
                Leire = 0,
                Silt = 1,
                Sand = 0,
                Grus = 0,
                Skjellsand = 0.5f
            },
            new BBlotbunn
            {
                Id = blotbunnId10,
                Leire = 1,
                Silt = 1,
                Sand = 0,
                Grus = 0,
                Skjellsand = 0.5f
            }
        ];
        
        await context.BBlotbunns.AddRangeAsync(blotbunns);
        await context.SaveChangesAsync();
        

        // Seed BHardbunn
        Guid hardbunnId1 = Guid.Parse("51042d7a-4b05-425c-a4b7-f373ba21a0be");
        Guid hardbunnId2 = Guid.Parse("d0d63f82-bc2c-4f70-94eb-5be5f9ae7931");
        
        
        Guid hardbunnId3 = Guid.Parse("15a4e651-194b-45d9-bb5d-11d68438607a");
        Guid hardbunnId4 = Guid.Parse("8bc8afee-023f-4462-b5cc-2578474edf94");
        Guid hardbunnId5 = Guid.Parse("b27ad3f6-e871-4321-9f99-c4d47f7f3801");
        Guid hardbunnId6 = Guid.Parse("b482e581-96ae-49bc-9174-01207f308765");

        List<BHardbunn> hardbunns =
        [
            new BHardbunn
            {
                Id = hardbunnId1,
                Steinbunn = 0,
                Fjellbunn = 1
            },
            new BHardbunn
            {
                Id = hardbunnId2,
                Steinbunn = 0,
                Fjellbunn = 1
            },
            
            //New
            new BHardbunn
            {
                Id = hardbunnId3,
                Steinbunn = 1,
                Fjellbunn = 0
            },
            new BHardbunn
            {
                Id = hardbunnId4,
                Steinbunn = 1,
                Fjellbunn = 0
            },
            new BHardbunn
            {
                Id = hardbunnId5,
                Steinbunn = 0,
                Fjellbunn = 1
            },
            new BHardbunn
            {
                Id = hardbunnId6,
                Steinbunn = 1,
                Fjellbunn = 0
            }
        ];

        await context.BHardbunns.AddRangeAsync(hardbunns);
        await context.SaveChangesAsync();
        
        
        // Seed BSediment
        Guid sedimentId1 = Guid.Parse("06290e72-2e6b-409b-bfce-6295a48d900c");
        Guid sedimentId3 = Guid.Parse("474b0557-6ee4-41e5-83bc-9c5a00c9a65c");
        Guid sedimentId5 = Guid.Parse("27fca494-95e9-48be-aaa3-3578ecbd1a39");
        
        Guid sedimentId7 = Guid.Parse("fd146f09-8721-4b2d-ac65-e1486fa6a92c");
        Guid sedimentId9 = Guid.Parse("82e4aa94-26a0-4b4f-af6b-ab868e1cea1c");
        Guid sedimentId11 = Guid.Parse("c66f82d7-9a80-4594-bfe1-b0f89503eb93");
        Guid sedimentId13 = Guid.Parse("6de68aa0-107b-48f7-a926-3cdac74e3d0f");
        Guid sedimentId15 = Guid.Parse("888d7932-d94a-41b9-9ab7-cbd02bc9d312");

        List<BSediment> sediments =
        [
            new BSediment()
            {
                Id = sedimentId1,
                Ph = 6.6f,
                Eh = -239.4f,
                Temperatur = 10.5f,
            },
            new BSediment()
            {
                Id = sedimentId3,
                Ph = 7.4f,
                Eh = -19.2f,
                Temperatur = -2.5f,
            },
            new BSediment()
            {
                Id = sedimentId5,
                Ph = 6.9f,
                Eh = -313.0f,
                Temperatur = 9.5f,
            },
            
            //New
            new BSediment
            {
                Id = sedimentId7,
                Ph = 6.8f,
                Eh = -50.6f,
                Temperatur = -1.2f
            },
            new BSediment
            {
                Id = sedimentId9,
                Ph = 7.2f,
                Eh = 200.3f,
                Temperatur = 9.5f
            },
            new BSediment
            {
                Id = sedimentId11,
                Ph = 7.4f,
                Eh = 100.2f,
                Temperatur = 1.5f
            },
            new BSediment
            {
                Id = sedimentId13,
                Ph = 7.4f,
                Eh = 100.2f,
                Temperatur = 1.5f
            },
            new BSediment
            {
                Id = sedimentId15,
                Ph = 7.4f,
                Eh = 100.2f,
                Temperatur = 1.5f
            }
        ];
        
        await context.BSediments.AddRangeAsync(sediments);
        await context.SaveChangesAsync();
        
        
        // Seed BSensorisk
        Guid sensoriskId1 = Guid.Parse("02843db6-6a65-4f45-85c2-4537096686e6");
        Guid sensoriskId2 = Guid.Parse("c1fce4f2-8c0e-4c62-9e0b-278dfb4e0218");
        Guid sensoriskId3 = Guid.Parse("da077591-ab46-44b2-b3ce-fa032fc60696");
        Guid sensoriskId5 = Guid.Parse("a6d80dc5-3911-4559-b092-54645be31a63");
        
        //New
        Guid sensoriskId7 = Guid.Parse("90d3259c-42c6-4974-9201-3c652f349e4c");
        Guid sensoriskId8 = Guid.Parse("948ed85e-870a-44ef-8546-516083013c2d");
        Guid sensoriskId9 = Guid.Parse("bf9edf36-c9ad-44a0-bece-c5a1171fecf0");
        Guid sensoriskId13 = Guid.Parse("91b66345-b0be-467d-a70f-14d2205757db");
        Guid sensoriskId15 = Guid.Parse("71dca13a-9c69-43bd-a153-190be96d2d4f");

        
        List<BSensorisk> sensorisks =
        [
            new BSensorisk
            {
                Id = sensoriskId1,
                Gassbobler = 0,
                Farge = 2,
                Lukt = 2,
                Konsistens = 2,
                Grabbvolum = 1,
                Tykkelseslamlag = 0,
            },
            new BSensorisk
            {
                Id = sensoriskId2,
                Gassbobler = 0,
                Farge = 2,
                Lukt = 0,
                Konsistens = 2,
                Grabbvolum = 0,
                Tykkelseslamlag = 0,
            },
            new BSensorisk
            {
                Id = sensoriskId3,
                Gassbobler = 0,
                Farge = 2,
                Lukt = 0,
                Konsistens = 2,
                Grabbvolum = 0,
                Tykkelseslamlag = 0,
            },
            new BSensorisk
            {
                Id = sensoriskId5,
                Gassbobler = 0,
                Farge = 2,
                Lukt = 2,
                Konsistens = 2,
                Grabbvolum = 1,
                Tykkelseslamlag = 0,
            },
            
            //New
            new BSensorisk
            {
                Id = sensoriskId7,
                Gassbobler = 0,
                Farge = 0,
                Lukt = 0,
                Konsistens = 2,
                Grabbvolum = 1,
                Tykkelseslamlag = 0,
            },
            new BSensorisk
            {
                Id = sensoriskId8,
                Gassbobler = 0,
                Farge = 0,
                Lukt = 2,
                Konsistens = 2,
                Grabbvolum = 0,
                Tykkelseslamlag = 0,
            },
            new BSensorisk
            {
                Id = sensoriskId9,
                Gassbobler = 0,
                Farge = 2,
                Lukt = 2,
                Konsistens = 2,
                Grabbvolum = 0,
                Tykkelseslamlag = 0
            },
            new BSensorisk
            {
                Id = sensoriskId13,
                Gassbobler = 0,
                Farge = 2,
                Lukt = 2,
                Konsistens = 2,
                Grabbvolum = 0,
                Tykkelseslamlag = 0
            },
            new BSensorisk
            {
                Id = sensoriskId15,
                Gassbobler = 0,
                Farge = 2,
                Lukt = 2,
                Konsistens = 2,
                Grabbvolum = 0,
                Tykkelseslamlag = 0
            }
        ];
        
        await context.BSensorisks.AddRangeAsync(sensorisks);
        await context.SaveChangesAsync();
        

        // Seed BDyr
        Guid dyrId1 = Guid.Parse("ffcf1a63-5530-4163-a38b-b34df2406979");
        
        //New
        Guid dyrId2 = Guid.Parse("7cafe42f-8a1f-4edd-b694-dbb538939262");
        Guid dyrId3 = Guid.Parse("629fa1c0-eb53-4998-8ae8-7d540a684709");
        Guid dyrId4 = Guid.Parse("43626c57-71e3-49ab-9359-dc9af7423d96");
        Guid dyrId5 = Guid.Parse("5810b46a-b259-487a-871c-b3030a745494");
        
        List<BDyr> dyrs =
        [
            new BDyr
            {
                Id = dyrId1,
                Pigghunder = "1",
                Krepsdyr = "3",
                Skjell = "50+",
                Borstemark = "50+",
                Arter = "OP"
            },
            
            //New
            new BDyr
            {
                Id = dyrId2,
                Pigghunder = "50+",
                Krepsdyr = "10",
                Skjell = "3",
                Borstemark = "30+",
                Arter = "CC"
            },
            new BDyr
            {
                Id = dyrId3,
                Pigghunder = "50+",
                Krepsdyr = "10",
                Skjell = "",
                Borstemark = "30+",
                Arter = "TH"
            },
            new BDyr
            {
                Id = dyrId4,
                Pigghunder = "30+",
                Krepsdyr = "",
                Skjell = "50+",
                Borstemark = "30+",
                Arter = "SL"
            },
            new BDyr
            {
                Id = dyrId5,
                Pigghunder = "30+",
                Krepsdyr = "30+",
                Skjell = "",
                Borstemark = "30+",
                Arter = "SM, V"
            }
        ];
        
        await context.BDyrs.AddRangeAsync(dyrs);
        await context.SaveChangesAsync();
        
        
        // Seed BUndersokelse
        Guid undersokelseId1 = Guid.Parse("3f7e477f-a26e-4b5b-93f3-546b6be693d1");
        Guid undersokelseId2 = Guid.Parse("6aa7a8eb-1491-4ebe-9901-09553a9f71f8");
        Guid undersokelseId3 = Guid.Parse("bdc31f24-67dd-4544-8724-d61577cfe783");
        Guid undersokelseId4 = Guid.Parse("7ba61e29-2fb4-4d88-9820-7face08475ed");
        Guid undersokelseId5 = Guid.Parse("2e021c0e-dc0b-4580-9b64-dcd2f04a4e38");
        
        //New
        Guid undersokelseId6 = Guid.Parse("fdba2a3e-900c-4901-be48-326b4e816c96");
        Guid undersokelseId7 = Guid.Parse("956ea28e-31f7-4aea-9150-318c5452e8e3");
        Guid undersokelseId8 = Guid.Parse("bfe67765-2ef6-4f64-b230-6b28d62c2f0e");
        Guid undersokelseId9 = Guid.Parse("fae240e4-ba3b-429a-96c0-f58cd01ed509");
        
        

        IEnumerable<BUndersokelse> undersokelses =
        [
            new BUndersokelse()
            {
                Id = undersokelseId1,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId1,
                Feltdato = new DateOnly(2025, 4, 8),
                AntallGrabbhugg = 3,
                GrabbhastighetGodkjent = true,
                BlotbunnId = blotbunnId1,
                HardbunnId = null,
                SedimentId = sedimentId1, 
                SensoriskId = sensoriskId1,
                Beggiatoa = false,
                Forrester = true,
                Fekalier = false,
                DyrId = dyrId1,
                Merknader = "Litt slam, rester av anleggsrens, terrestrisk materiale, detritus",
                DatoRegistrert = new DateTime(2025, 4, 8, 9, 0, 0),
                DatoEndret = DateTime.Now,
            },
            new BUndersokelse()
            {
                Id = undersokelseId2,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId1,
                Feltdato = new DateOnly(2025, 4, 8),
                AntallGrabbhugg = 2,
                GrabbhastighetGodkjent = true,
                BlotbunnId = null,
                HardbunnId = hardbunnId2,
                SedimentId = null, 
                SensoriskId = sensoriskId2,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = false,
                DyrId = null,
                Merknader = " Litt slam, rester av for, rester av anleggsrens, detritus",
                DatoRegistrert = new DateTime(2025, 4, 8, 9, 0, 0),
                DatoEndret = DateTime.Now,
            },
            new BUndersokelse()
            {
                Id = undersokelseId3,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId1,
                Feltdato = new DateOnly(2025, 4, 8),
                AntallGrabbhugg = 1,
                GrabbhastighetGodkjent = true,
                BlotbunnId = blotbunnId3,
                HardbunnId = null,
                SedimentId = sedimentId3, 
                SensoriskId = sensoriskId3,
                Beggiatoa = false,
                Forrester = false,
                Fekalier = true,
                DyrId = dyrId2,
                Merknader = "pH/Eh målt i liten boks, stein i åpningen av grabb, detritus",
                DatoRegistrert = new DateTime(2025, 4, 8, 9, 0, 0),
                DatoEndret = DateTime.Now,
            },
            new BUndersokelse()
            {
                Id = undersokelseId4,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId2,
                Feltdato = new DateOnly(2025, 4, 25),
                AntallGrabbhugg = 3,
                GrabbhastighetGodkjent = true,
                BlotbunnId = null,
                HardbunnId = hardbunnId1,
                SedimentId = null, 
                SensoriskId = null,
                Beggiatoa = true,
                Forrester = true,
                Fekalier = false,
                Dyr = null,
                Merknader = "Rester av anleggsrens, detritus",
                DatoRegistrert = new DateTime(2025, 4, 25, 9, 0, 0),
                DatoEndret = DateTime.Now,
            },
            new BUndersokelse()
            {
                Id = undersokelseId5,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId2,
                Feltdato = new DateOnly(2025, 4, 25),
                AntallGrabbhugg = 1,
                GrabbhastighetGodkjent = true,
                BlotbunnId = blotbunnId5,
                HardbunnId = null,
                SedimentId = sedimentId5, 
                SensoriskId = sensoriskId5,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = false,
                DyrId = dyrId5,
                Merknader = "Rester av anleggsrens, terrestrisk materiale",
                DatoRegistrert = new DateTime(2025, 4, 25, 9, 0, 0),
                DatoEndret = DateTime.Now,
                Korrigeringer = "Ola Solberg har endret Antall Grabbhugg feltet, fra 2 til 1, 26.04.2025 15:30"
            },
            new BUndersokelse()
            {
                Id = undersokelseId6,
                ProsjektId = prosjektId2,
                PreinfoId = preinfoId3,
                Feltdato = new DateOnly(2025, 4, 28),
                AntallGrabbhugg = 2,
                GrabbhastighetGodkjent = true,
                BlotbunnId = null,
                HardbunnId = hardbunnId3,
                SedimentId = null, 
                SensoriskId = null,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = false,
                Dyr = null,
                Merknader = "Litt slam",
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now,
            },
            
            //new
            new BUndersokelse()
            {
                Id = undersokelseId7,
                ProsjektId = prosjektId2,
                PreinfoId = preinfoId3,
                Feltdato = new DateOnly(2025, 4, 28),
                AntallGrabbhugg = 3,
                GrabbhastighetGodkjent = false,
                BlotbunnId = blotbunnId7,
                HardbunnId = null,
                SedimentId = sedimentId7,
                SensoriskId = sensoriskId7,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = false,
                DyrId = dyrId3,
                Merknader = "D",
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now,
                Korrigeringer = "Tommy har endret forrester fra true til false 1.februar"
            },
            new BUndersokelse()
            {
                Id = undersokelseId8,
                ProsjektId = prosjektId2,
                PreinfoId = preinfoId4,
                Feltdato = new DateOnly(2025, 5, 1),
                AntallGrabbhugg = 2,
                GrabbhastighetGodkjent = true,
                BlotbunnId = null,
                HardbunnId = hardbunnId6,
                SedimentId = null,
                SensoriskId = sensoriskId8,
                Beggiatoa = false,
                Forrester = true,
                Fekalier = true,
                Dyr = null,
                Merknader = "TM",
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now,
                Korrigeringer = "Isak har endret antall grabbhugg fra 3 til 2 25.april"
            },
            new BUndersokelse()
            {
                Id = undersokelseId9,
                ProsjektId = prosjektId2,
                PreinfoId = preinfoId4,
                Feltdato = new DateOnly(2025, 5, 15),
                AntallGrabbhugg = 2,
                GrabbhastighetGodkjent = false,
                BlotbunnId = blotbunnId10,
                HardbunnId = null,
                SedimentId = sedimentId9,
                SensoriskId = sensoriskId9,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = true,
                DyrId = dyrId4,
                Merknader = "RAR",
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now,
                Korrigeringer = "Tine har lagt til merknader RAR 11.mars"
            },
            
        ];
        
        await context.BUndersokelses.AddRangeAsync(undersokelses);
        await context.SaveChangesAsync();
        
        
        // Seed BStasjon
        Guid stasjonId1 = Guid.Parse("03db101b-581b-46da-8bc8-726c1d9d31aa");
        Guid stasjonId2 = Guid.Parse("14ce5a18-4652-4cd8-b8d2-b696c8846c60");
        Guid stasjonId3 = Guid.Parse("6e31d25f-085f-445a-831e-14498482a223");
        Guid stasjonId4 = Guid.Parse("c0a748aa-3383-4db9-9176-17b84427a48a");
        Guid stasjonId5 = Guid.Parse("5f463dab-1a95-4346-a2bf-e3d9fc4ea5f7");
        
        
        //New
        Guid stasjonId6 = Guid.Parse("e3f192af-7562-47bf-a980-1839638280d9");
        Guid stasjonId7 = Guid.Parse("dbfc3976-ff89-49ce-9c99-d346e825f9f6");
        Guid stasjonId8 = Guid.Parse("ccc29757-e3ed-4101-9c3e-18e2ca132dcb");
        Guid stasjonId9 = Guid.Parse("37add753-974c-46ce-a6a3-29ed915adf68");
        Guid stasjonId10 = Guid.Parse("8927784e-e12a-42ec-8544-3ea0a2b53365");
        Guid stasjonId11 = Guid.Parse("2433d779-7410-40e2-8677-d908036677e1");
        Guid stasjonId12 = Guid.Parse("bccb52d2-68ab-4230-a850-9642cca40b8c");

        List<BStasjon> stasjons =
        [
            new BStasjon
            {
                Id = stasjonId1,
                ProsjektId = prosjektId1,
                Nummer = 1,
                KoordinatNord = "68°46.774",
                KoordinatOst = "17°16.994",
                Dybde = 105,
                Analyser = "Parameter I, II og III",
                ProvetakingsplanId = provetakingsplanId1,
                UndersokelseId = undersokelseId1
            },
            new BStasjon
            {
                Id = stasjonId2,
                ProsjektId = prosjektId1,
                Nummer = 2,
                KoordinatNord = "68°46.798",
                KoordinatOst = "17°16.912",
                Dybde = 105,
                Analyser = "Parameter I, II og III",
                ProvetakingsplanId = provetakingsplanId1,
                UndersokelseId = undersokelseId2
            },
            new BStasjon
            {
                Id = stasjonId3,
                ProsjektId = prosjektId1,
                Nummer = 3,
                KoordinatNord = "68°46.822",
                KoordinatOst = "17°16.832",
                Dybde = 110,
                Analyser = "Parameter I, II og III",
                ProvetakingsplanId = provetakingsplanId1,
                UndersokelseId = undersokelseId3
            },
            new BStasjon
            {
                Id = stasjonId4,
                ProsjektId = prosjektId1,
                Nummer = 4,
                KoordinatNord = "68°46.843",
                KoordinatOst = "17°16.753",
                Dybde = 121,
                Analyser = "Parameter I, II og III",
                ProvetakingsplanId = provetakingsplanId1,
                UndersokelseId = undersokelseId4
            },
            new BStasjon
            {
                Id = stasjonId5,
                ProsjektId = prosjektId1,
                Nummer = 5,
                KoordinatNord = "68°46.867",
                KoordinatOst = "17°16.670",
                Dybde = 142,
                Analyser = "Parameter I, II og III",
                ProvetakingsplanId = provetakingsplanId1,
                UndersokelseId = undersokelseId5
            },
            
            //new
            new BStasjon
            {
                Id = stasjonId6,
                ProsjektId = prosjektId2,
                Nummer = 1,
                KoordinatNord = "56°46.230",
                KoordinatOst = "61°20.870",
                Dybde = 30,
                Analyser = "Parameter I, II",
                ProvetakingsplanId = provetakingsplanId2,
                UndersokelseId = undersokelseId6
            },
            new BStasjon
            {
                Id = stasjonId7,
                ProsjektId = prosjektId2,
                Nummer = 2,
                KoordinatNord = "24°08.400",
                KoordinatOst = "34°15.900",
                Dybde = 21,
                Analyser = "Parameter I",
                ProvetakingsplanId = provetakingsplanId2,
                UndersokelseId = undersokelseId7
            },
            new BStasjon
            {
                Id = stasjonId8,
                ProsjektId = prosjektId2,
                Nummer = 3,
                KoordinatNord = "66°06.400",
                KoordinatOst = "38°12.900",
                Dybde = 46,
                Analyser = "Parameter II og III",
                ProvetakingsplanId = provetakingsplanId2,
                UndersokelseId = undersokelseId8
            },
            new BStasjon
            {
                Id = stasjonId9,
                ProsjektId = prosjektId2,
                Nummer = 4,
                KoordinatNord = "68°28.400",
                KoordinatOst = "18°32.900",
                Dybde = 26,
                Analyser = "Parameter II og III",
                ProvetakingsplanId = provetakingsplanId2,
                UndersokelseId = undersokelseId9
            },
            new BStasjon
            {
                Id = stasjonId10,
                ProsjektId = prosjektId3,
                Nummer = 1,
                KoordinatNord = "50°20.400",
                KoordinatOst = "18°42.900",
                Dybde = 46,
                Analyser = "Parameter III",
                ProvetakingsplanId = provetakingsplanId3,
                UndersokelseId = null
            },
            new BStasjon
            {
                Id = stasjonId11,
                ProsjektId = prosjektId3,
                Nummer = 2,
                KoordinatNord = "38°12.900",
                KoordinatOst = "66°06.400",
                Dybde = 22,
                Analyser = "Parameter I",
                ProvetakingsplanId = provetakingsplanId3,
                UndersokelseId = null
            },
            new BStasjon
            {
                Id = stasjonId12,
                ProsjektId = prosjektId3,
                Nummer = 3,
                KoordinatNord = "34°20.900",
                KoordinatOst = "22°41.400",
                Dybde = 33,
                Analyser = "Parameter I og III",
                ProvetakingsplanId = provetakingsplanId3,
                UndersokelseId = null
            },
        ];

        await context.BStasjons.AddRangeAsync(stasjons);
        await context.SaveChangesAsync();
        
        // Seed BTistand
        Guid tilstandId1 = Guid.Parse("e716cc15-d2bd-4ead-9da6-8be75b939c20");
        
        Guid tilstandId2 = Guid.Parse("d9b63661-049f-4d1d-bb86-a8c1277e4634");
        Guid tilstandId3 = Guid.Parse("27b6e4f3-01c7-470d-9934-0ede9092e199");
        Guid tilstandId4 = Guid.Parse("e6d4d8ae-ad84-40d9-b489-7c688169ea4e");

        List<BTilstand> tilstands =
        [
            new BTilstand
            {
                Id = tilstandId1,
                ProsjektId = prosjektId4,
                IndeksGr2 = 2.5f,
                TilstandGr2 = 3,
                IndeksGr3 = 0.968f,
                TilstandGr3 = 1,
                IndeksLokalitet = 1.572f,
                TilstandLokalitet = 2
            },
            new BTilstand
            {
                Id = tilstandId3,
                ProsjektId = prosjektId6,
                IndeksGr2 = 2.5f,
                TilstandGr2 = 3,
                IndeksGr3 = 0.968f,
                TilstandGr3 = 1,
                IndeksLokalitet = 1f,
                TilstandLokalitet = 1
            },
            new BTilstand
            {
                Id = tilstandId4,
                ProsjektId = prosjektId7,
                IndeksGr2 = 2.5f,
                TilstandGr2 = 3,
                IndeksGr3 = 0.968f,
                TilstandGr3 = 1,
                IndeksLokalitet = 2.5f,
                TilstandLokalitet = 3
            },
            new BTilstand
            {
                Id = tilstandId2,
                ProsjektId = prosjektId8,
                IndeksGr2 = 2.5f,
                TilstandGr2 = 3,
                IndeksGr3 = 0.968f,
                TilstandGr3 = 1,
                IndeksLokalitet = 3.5f,
                TilstandLokalitet = 4
            }
        ];
        
        await context.BTilstands.AddRangeAsync(tilstands);
        await context.SaveChangesAsync();
        
        
        //Seed BRapporter
        Guid rapportId1 = Guid.Parse("e642ac32-2ba9-47a9-980f-0c2feb76bb49");
        Guid rapportId2 = Guid.Parse("1a1041dc-81ea-4dd4-a18a-6f465f88ede0");
        
        List<BRapporter> rapporters =
        [
            new BRapporter()
            {
                Id = rapportId1,
                ProsjektId = prosjektId1,
                ArkNavn = 2,
                Datogenerert = new DateTime(2025, 2, 22, 9, 48, 18)
            },
            new BRapporter()
            {
                Id = rapportId2,
                ProsjektId = prosjektId2,
                ArkNavn = 2,
                Datogenerert = new DateTime(2025, 2, 22, 10, 19, 04)
            }
        ];
        
        await context.BRapporters.AddRangeAsync(rapporters);
        await context.SaveChangesAsync();
    }
}