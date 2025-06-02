
# Instruksjon for oppsett og kjøring av systemet

## Forhåndskrav – installasjoner

1. Last ned og installer pgAdmin 4 (verktøy for PostgreSQL):  
   https://www.pgadmin.org/download/

2. Last ned og installer PostgreSQL (database):  
   https://www.postgresql.org/download/

3. Last ned og installer JetBrains Rider (IDE):  
   https://www.jetbrains.com/rider/

4. Installer .NET SDK 8 (kreves for å bygge og kjøre prosjektet):  
   https://dotnet.microsoft.com/en-us/download/dotnet/8.0

---

## Konfigurasjon av database

1. Åpne pgAdmin 4 og opprett en lokal server.
2. Opprett en database med navn seaeco, og bruk standard public schema.
3. Åpne Query Tool i databasen seaeco.
4. Åpne medfølgende «SeaEco DB v4.0.sql» fil og kjør den for å sette opp databasen:  
   Execute script (Ctrl+Enter).

---

## Tillegg – databaseoversikt (ERD)

I mappen DB v4.0 finner dere to filer knyttet til databasens struktur (ERD):

- ERD SeaEco DB v4.0.png – et bilde som viser databasens ER-diagram.
- ERD SeaEco DB v4.0 (dokument) – selve ERD-filen som kan åpnes i pgAdmin 4.

Slik åpner du ERD-filen i pgAdmin 4:

1. Åpne pgAdmin 4.
2. Høyreklikk på databasen og velg 'ERD for database'.
3. Velg 'Open File' og finn filen ERD SeaEco DB v4.0.
4. Diagrammet vil da vises med tabeller og relasjoner.

**Merk:** I databasen finnes det én tabell som ikke er i bruk – `b_undersokelseslogg`. Denne tabellen ble opprettet med tanke på automatisk logging av endringer i undersøkelser, men funksjonaliteten ble ikke implementert i prosjektet.

---

## Åpne og kjør prosjektet

1. Åpne løsningen `SeaEco.sln` i Rider.
2. Gå til prosjektet `SeaEco.Server`, og åpne filen `appsettings.json`.
3. Sjekk verdien for `"LocalConnection"` – tilpass den etter din lokale database.  
   Merk: standardverdien inneholder ikke `password=`, det må legges til hvis nødvendig.

---

## Kjøring av systemet

- Kjør **Server** i Rider (HTTPS)
- Kjør **Client** i Rider (HTTPS)

Etter første kjøring er systemet tilgjengelig med testdata.

---

## Testbrukere – innloggingsinformasjon

### Admin brukere

- Brukernavn: akvatisk@sea-eco.no  
  Passord: 11111111
- Brukernavn: rikke@sea-eco.no  
  Passord: 11111111
- Brukernavn: admin3@test.no  
  Passord: 11111111

### Vanlige brukere

- Brukernavn: bruker2@test.no  
  Passord: 11111111
- Brukernavn: bruker3@test.no  
  Passord: 11111111
- Brukernavn: bruker4@test.no  
  Passord: 11111111

Se også egne opplysninger i `DbSeeder.cs` i mappen `Infrastructure` i `SeaEco.Server`-prosjektet.

---

## Seeding – kontroll og tilpasning

Hvis du ønsker at data som er seedet inn i databasen ved første oppstart, samt data du selv har lagt inn under testing, skal beholdes ved neste oppstart – da må du kommentere ut metodekallet i `Program.cs` i `SeaEco.Server`-prosjektet som vist under.

Kommenter ut følgende linje for å unngå at databasen nullstilles ved hver oppstart. Kallet ligger på linje 180 i filen:

```csharp
void SeedData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    DbSeeder seeder = new DbSeeder();
    //seeder.SeedData(scope.ServiceProvider.GetRequiredService<AppDbContext>(), true).GetAwaiter().GetResult();
}
```

Dette er fordi seedingprosessen automatisk tømmer alle tabeller ved hver oppstart av serveren ved hjelp av følgende kode:

```csharp
if (regenerate)
{
    context.TruncateAllTablesPostgres();
}
```

Hvis denne metoden ikke kommenteres ut, vil databasen nullstilles ved hver oppstart, og tidligere innlagte data vil gå tapt.

---

## Hvordan å fjerne testdata etter ferdig testing

1. Stopp seeding av databasen ved å kommentere ut følgende kallet på linje 180 i `Program.cs` i `SeaEco.Server`-prosjektet:

```csharp
void SeedData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    DbSeeder seeder = new DbSeeder();
    //seeder.SeedData(scope.ServiceProvider.GetRequiredService<AppDbContext>(), true).GetAwaiter().GetResult();
}
```

2. Åpne `pgAdmin 4` → velg **Query Tool** i databasen `seaeco` → lim inn og kjør denne kommandoen:

```sql
DO $$ 
DECLARE 
  r RECORD; 
BEGIN 
  FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = 'public') LOOP 
    EXECUTE 'TRUNCATE TABLE public.' || quote_ident(r.tablename) || ' CASCADE'; 
  END LOOP; 
END $$;
```

3. Gå til prosjektmappen `wwwroot` i `SeaEco.Server`-prosjektet og slett alle lastet ned bildefiler i `images`-mappen og alle genererte rapporter i `generated`-mappen som ble opprettet under seeding eller testing.

---

## Viktig informasjon om lagring

Alle rapporter og bilder som genereres i programmet, lagres fysisk i `wwwroot`-mappen.  
I databasen lagres kun metadata.

---

## Ved videre distribusjon / kommersiell bruk

Når dere skal bruke ekstern server:  
Oppdater verdien for `"DefaultConnection"` til deres eksterne database i `appsettings.json` i `SeaEco.Server`-prosjektet.  
I tillegg må dere også oppdatere tilkoblingsstrengen i `Program.cs` i `SeaEco.Server`-prosjektet og i `AppDbContext.cs` i `SeaEco.EntityFramework`-prosjektet, slik at `AppDbContext` peker til riktig database.

Oppdater følgende i `appsettings.json` i `Server`-prosjektet:

- `"DefaultConnection"` – ved bytte til ekstern server
- `"SmtpOptions"` – bruk Sea Eco e-postkonto og endre alle options til dette – svært obligatorisk
- `"ReportOptions"`
   - Her er `"NonCommercialPersonalName": "NonCommercialS"` viktig:  
     Dette må endres ved kommersiell bruk pga lisensiering i [EPPlus-biblioteket](https://www.epplussoftware.com/en/Developers)
