using Microsoft.EntityFrameworkCore;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Services.HashService;

namespace SeaEco.Server.Infrastructure;

public sealed class DbSeeder
{
    public async Task SeedData(AppDbContext context)
    {
        context.TruncateAllTablesPostgres();
        
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
        
        await context.Lokalitets.AddRangeAsync(lokalitets);
        await context.SaveChangesAsync();
        
        
        // Seed Kunder 
        Guid kundeId1 = Guid.Parse("0f7a3a3e-55c9-4317-93eb-ed8b5741e04e");
        Guid kundeId2 = Guid.Parse("7b4fc6a6-4a5b-47f7-9ca0-58de6ad622e4");
        Guid kundeId3 = Guid.Parse("80cd9f7e-dd53-40b9-9e3a-f57b31819a97");

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
            }
        ];
        
        await context.Kundes.AddRangeAsync(kundes);
        await context.SaveChangesAsync();
        
        
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
                Prosjektstatus = 3,
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
                Prosjektstatus = 1,
                Mtbtillatelse = 200,
                Produksjonsstatus = 2,
                KundeId = kundeId2,
                LokalitetId = lokalitetId2,
                PoId = "407",
                ProsjektIdSe = "SE-25-BU-2",
                ProsjektansvarligId = Guid.Parse("8fffdaa4-7dfe-4d78-a28b-b80558d542b6")
            }
        ];
        
        await context.BProsjekts.AddRangeAsync(prosjekts);
        await context.SaveChangesAsync();
        
        //Seed BProvetakingsplan
        Guid provetakingsplanId1 = Guid.Parse("82ca018f-f197-42ae-832a-01fdc895f562"); 
        
        List<BProvetakningsplan> provetakningsplans =
        [
            new BProvetakningsplan
            {
                Id = provetakingsplanId1,
                ProsjektId = prosjektId1,
                Planlagtfeltdato = new DateOnly(2025, 3, 30),
                PlanleggerId = adminId 
            }
        ];
        
        await context.BProvetakningsplans.AddRangeAsync(provetakningsplans);
        await context.SaveChangesAsync();
        
        
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

        await context.BPreinfos.AddRangeAsync(preinfos);
        await context.SaveChangesAsync();
        
        
        // Seed BBlotbunn
        Guid blotbunnId1 = Guid.Parse("830eebc8-6d86-40ad-88ed-3cbe3e43e0bc");
        Guid blotbunnId2 = Guid.Parse("37e05b0b-0a82-45c3-a304-7e8426e1bf53");
        Guid blotbunnId3 = Guid.Parse("0d3724ca-3887-4bdc-b718-5bbe52f3e80d");
        
        
        List<BBlotbunn> blotbunns =
        [
            new BBlotbunn
            {
                Id = blotbunnId1,
                Leire = 0,
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
                Sand = 0,
                Grus = 1,
                Skjellsand = 0
            },
            new BBlotbunn
            {
                Id = blotbunnId3,
                Leire = 0,
                Silt = 1,
                Sand = 0,
                Grus = 1,
                Skjellsand = 0
            }
        ];
        
        await context.BBlotbunns.AddRangeAsync(blotbunns);
        await context.SaveChangesAsync();
        

        // Seed BHardbunn
        Guid hardbunnId1 = Guid.Parse("51042d7a-4b05-425c-a4b7-f373ba21a0be");
        Guid hardbunnId2 = Guid.Parse("d0d63f82-bc2c-4f70-94eb-5be5f9ae7931");

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
            }
        ];

        await context.BHardbunns.AddRangeAsync(hardbunns);
        await context.SaveChangesAsync();
        
        // Seed BSensorisk
        Guid sensoriskId1 = Guid.Parse("02843db6-6a65-4f45-85c2-4537096686e6");
        
        List<BSensorisk> sensorisks =
        [
            new BSensorisk
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
            }
        ];
        
        await context.BSensorisks.AddRangeAsync(sensorisks);
        await context.SaveChangesAsync();
        

        // Seed BDyr
        Guid dyrId1 = Guid.Parse("ffcf1a63-5530-4163-a38b-b34df2406979");
        
        
        List<BDyr> dyrs =
        [
            new BDyr
            {
                Id = dyrId1,
                Pigghunder = "1",
                Krepsdyr = "3",
                Skjell = "50+",
                Borstemark = "50+",
                Arter = "Andre arter: 2"
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
        Guid undersokelseId6 = Guid.Parse("fdba2a3e-900c-4901-be48-326b4e816c96");

        IEnumerable<BUndersokelse> undersokelses =
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
                DatoEndret = DateTime.Now,
                BlotbunnId = blotbunnId1,
                SensoriskId = sensoriskId1,
                DyrId = dyrId1,
                Merknader = "Rester av anleggsmateriale"
            },
            new BUndersokelse()
            {
                Id = undersokelseId2,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId1,
                Feltdato = DateOnly.FromDateTime(DateTime.Now),
                AntallGrabbhugg = 8,
                GrabbhastighetGodkjent = true,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = false,
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now,
                BlotbunnId = blotbunnId2
            },
            new BUndersokelse()
            {
                Id = undersokelseId3,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId1,
                Feltdato = DateOnly.FromDateTime(DateTime.Now),
                AntallGrabbhugg = 8,
                GrabbhastighetGodkjent = true,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = false,
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now,
                BlotbunnId = blotbunnId3
            },
            new BUndersokelse()
            {
                Id = undersokelseId4,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId1,
                Feltdato = DateOnly.FromDateTime(DateTime.Now),
                AntallGrabbhugg = 8,
                GrabbhastighetGodkjent = true,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = false,
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now,
                HardbunnId = hardbunnId1
            },
            new BUndersokelse()
            {
                Id = undersokelseId5,
                ProsjektId = prosjektId1,
                PreinfoId = preinfoId1,
                Feltdato = DateOnly.FromDateTime(DateTime.Now),
                AntallGrabbhugg = 8,
                GrabbhastighetGodkjent = true,
                Beggiatoa = true,
                Forrester = false,
                Fekalier = false,
                DatoRegistrert = DateTime.Now,
                DatoEndret = DateTime.Now,
                HardbunnId = hardbunnId2
            },
            new BUndersokelse()
            {
                Id = undersokelseId6,
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
        
        await context.BUndersokelses.AddRangeAsync(undersokelses);
        await context.SaveChangesAsync();
        
        
        // Seed BStasjon
        Guid stasjonId1 = Guid.Parse("03db101b-581b-46da-8bc8-726c1d9d31aa");
        Guid stasjonId2 = Guid.Parse("14ce5a18-4652-4cd8-b8d2-b696c8846c60");
        Guid stasjonId3 = Guid.Parse("6e31d25f-085f-445a-831e-14498482a223");
        Guid stasjonId4 = Guid.Parse("c0a748aa-3383-4db9-9176-17b84427a48a");
        Guid stasjonId5 = Guid.Parse("5f463dab-1a95-4346-a2bf-e3d9fc4ea5f7");

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
            }
        ];

        await context.BStasjons.AddRangeAsync(stasjons);
        await context.SaveChangesAsync();
        
    }
}