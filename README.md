# SeaEco

## Om prosjektet

SeaEco er en webbasert løsning for innsamling og rapportering av data fra B-undersøkelser i felt.  
Frontend er bygd med **Blazor WebAssembly (WASM)**, backend med **ASP.NET Core 8.0 Web API** og datalaget bruker **Entity Framework Core** mot **PostgreSQL**.

Målsetting:
- Digitalisere dagens papirskjemaer
- Effektivisere arbeidsprosesser knyttet til B-Undersøkelsen
- Automatisere beregninger av miljøtilstand og generering av Excel-rapporter

---

## Prerekvisitter

- .NET 8 SDK
- PostgreSQL v15+
- Visual Studio 2022 eller JetBrains Rider 


---

## Oppsett

### 1. Klon reposit

```bash
git clone https://github.com/TetianaVerkhalantseva/SeaEco.git
```

--- 
## VPN og databaseoppsett

Denne versjonen av applikasjon henter data fra ligger på UiT sin VPN. Databasen tillater kun noen få IPadresser. Derfor må applikasjonen kjøres enten på skolens nettverk via kabel eller via VPN.

vpn.uit.no/student
Logge på VM: for å logge på VM må man kjøre disse i terminalen:
ssh tetiana@10.239.120.212
Sea2015

Tilgang til postgres på VM gjennom postgresbrukeren admin:
1. Logge inn på VPN
2. Logge inn på VM for å sjekke at postgres kjører (kjør linjen : 'sudo systemctl start postgresql'. Hvis aktiv er alt ok, hvis ikke Kjør : 'sudo systemctl start postgresql')
3. Åpne en ny terminal på din maskin
4. kjør: 'psql -h 10.239.120.212 -U admin -d seaeco -W' (admin er brukeren, template1 er en av databasen som fulgte med ved innstalering av postgres og må endres til korrekt database når den er opprettet.)
   For brukeren admin er passordet=admin. 

--- 

## Kjør system
Run SeaEco.Server:https i IDE
Run SeaEco.Client: https i IDE

--- 
## Login
Admin bruker
Brukernavn: gruppe202520@gmail.com
Passord: 1111

Vanlig bruker
Brukernavn: annbrueab@gmail.com
password: 12345678

--- 
