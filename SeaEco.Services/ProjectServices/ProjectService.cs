using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Models.Project;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.ProjectServices;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    
    public ProjectService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> CreateProjectAsync(NewProjectDto dto)
    {
        
        // Sjekk om lokalitet finnes, ellers opprett
        var lokalitet = await _context.Lokalitets
            .FirstOrDefaultAsync(l => l.Lokalitetsnavn == dto.Lokalitetsnavn || l.LokalitetsId == dto.LokalitetsId);

        if (lokalitet == null)
        {
            lokalitet = new Lokalitet
            {
                Id = Guid.NewGuid(),
                Lokalitetsnavn = dto.Lokalitetsnavn,
                LokalitetsId = dto.LokalitetsId
            };
            _context.Lokalitets.Add(lokalitet);
            await _context.SaveChangesAsync();
        }
        
        if (!dto.ProsjektansvarligId.HasValue)
            throw new ArgumentException("ProsjektansvarligId må oppgis.");
        
        var ansvarligId = dto.ProsjektansvarligId.Value;
        
        var prosjekt = new BProsjekt
        {
            Id = Guid.NewGuid(),
            PoId = dto.PoId,
            KundeId = dto.KundeId,
            Kundekontaktperson = dto.Kundekontaktperson,
            Kundetlf = dto.Kundetlf,
            Kundeepost = dto.Kundeepost,
            LokalitetId = lokalitet.Id,
            Mtbtillatelse = dto.Mtbtillatelse,
            ProsjektansvarligId = ansvarligId,
            Merknad = dto.Merknad,
            Produksjonsstatus = (int)dto.Produksjonsstatus,
            Prosjektstatus      = (int)Prosjektstatus.Nytt,
            Datoregistrert = DateTime.Now
        };

        _context.BProsjekts.Add(prosjekt);
        await _context.SaveChangesAsync();

        return prosjekt.Id;
    }
    
    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        return await _context.BProsjekts
            .Include(p => p.Kunde)  
            .Include(p => p.Lokalitet)
            .Include(p => p.BTilstand)
            .Include(p => p.BPreinfos)  
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                PoId = p.PoId,
                KundeId = p.KundeId,
                Oppdragsgiver = p.Kunde.Oppdragsgiver, 
                Kundekontaktperson = p.Kundekontaktperson,
                Kundetlf = p.Kundetlf,
                Kundeepost = p.Kundeepost,
                LokalitetId = p.LokalitetId,
                Lokalitetsnavn = p.Lokalitet.Lokalitetsnavn,
                LokalitetsId = p.Lokalitet.LokalitetsId,
                Mtbtillatelse = p.Mtbtillatelse,
                ProsjektansvarligId = p.ProsjektansvarligId,
                Merknad = p.Merknad,
                Produksjonsstatus = (Produksjonsstatus)p.Produksjonsstatus,
                Prosjektstatus = (Prosjektstatus)p.Prosjektstatus,
                Tilstand = p.BTilstand != null
                    ? (Tilstand?)p.BTilstand.TilstandLokalitet
                    : null,
                Feltdatoer = p.BPreinfos
                    .Select(pi => pi.Feltdato)
                    .OrderBy(d => d)
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(Guid id)
    {
        var p = await _context.BProsjekts
            .Include(p => p.Lokalitet)
            .Include(p => p.BTilstand)
            .Include(p => p.BPreinfos) 
            .FirstOrDefaultAsync(p => p.Id == id);

        if (p == null)
            return null;

        return new ProjectDto
        {
            Id = p.Id,
            PoId = p.PoId,
            KundeId = p.KundeId,
            Kundekontaktperson = p.Kundekontaktperson,
            Kundetlf = p.Kundetlf,
            Kundeepost = p.Kundeepost,
            LokalitetId = p.LokalitetId,
            Lokalitetsnavn = p.Lokalitet.Lokalitetsnavn,
            LokalitetsId = p.Lokalitet.LokalitetsId,
            Mtbtillatelse = p.Mtbtillatelse,
            ProsjektansvarligId = p.ProsjektansvarligId,
            Merknad = p.Merknad,
            Produksjonsstatus = (Produksjonsstatus)p.Produksjonsstatus,
            Prosjektstatus = (Prosjektstatus)p.Prosjektstatus,
            Tilstand = p.BTilstand != null
                ? (Tilstand?)p.BTilstand.TilstandLokalitet
                : null,
            Feltdatoer = p.BPreinfos
                .Select(pi => pi.Feltdato)
                .OrderBy(d => d)
                .ToList(),
            AntallStasjoner = await _context.BUndersokelses
                .CountAsync(u => u.ProsjektId == p.Id)
        };
    }
    
    public async Task<ProjectDto> UpdateProjectAsync(Guid id, EditProjectDto dto)
    {
        var prosjekt = await _context.BProsjekts
            .Include(p => p.Lokalitet)
            .Include(p => p.BTilstand)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prosjekt == null)
            throw new KeyNotFoundException("Prosjekt ikke funnet.");

        if (dto.Lokalitetsnavn != prosjekt.Lokalitet.Lokalitetsnavn
            || dto.LokalitetsId   != prosjekt.Lokalitet.LokalitetsId)
        {
            var lokalitet = await _context.Lokalitets
                .FirstOrDefaultAsync(l => l.Lokalitetsnavn == dto.Lokalitetsnavn
                                          || l.LokalitetsId    == dto.LokalitetsId);
            if (lokalitet == null)
            {
                lokalitet = new Lokalitet
                {
                    Id             = Guid.NewGuid(),
                    Lokalitetsnavn = dto.Lokalitetsnavn,
                    LokalitetsId   = dto.LokalitetsId
                };
                _context.Lokalitets.Add(lokalitet);
                await _context.SaveChangesAsync();
            }
            prosjekt.LokalitetId = lokalitet.Id;
        }
        
        prosjekt.Kundekontaktperson = dto.Kundekontaktperson;
        prosjekt.Kundetlf = dto.Kundetlf;
        prosjekt.Kundeepost = dto.Kundeepost;
        prosjekt.Mtbtillatelse = dto.Mtbtillatelse;
        prosjekt.ProsjektansvarligId = dto.ProsjektansvarligId;
        prosjekt.Produksjonsstatus   = (int)dto.Produksjonsstatus;
        
        if (!string.IsNullOrWhiteSpace(dto.Merknad))
        {
            var nyKommentar = $"{dto.Merknad}";

            if (string.IsNullOrWhiteSpace(prosjekt.Merknad))
                prosjekt.Merknad = nyKommentar;
            else
                prosjekt.Merknad += $"\n{nyKommentar}";
        }
    
        await _context.SaveChangesAsync();
        
        return new ProjectDto
        {
            Id                   = prosjekt.Id,
            PoId                 = prosjekt.PoId,
            KundeId              = prosjekt.KundeId,
            Kundekontaktperson   = prosjekt.Kundekontaktperson,
            Kundetlf             = prosjekt.Kundetlf,
            Kundeepost           = prosjekt.Kundeepost,
            LokalitetId          = prosjekt.LokalitetId,
            Lokalitetsnavn       = prosjekt.Lokalitet.Lokalitetsnavn,
            LokalitetsId         = prosjekt.Lokalitet.LokalitetsId,
            Mtbtillatelse        = prosjekt.Mtbtillatelse,
            ProsjektansvarligId  = prosjekt.ProsjektansvarligId,
            Merknad              = prosjekt.Merknad,
            Produksjonsstatus    = (Produksjonsstatus)prosjekt.Produksjonsstatus,
            Prosjektstatus       = (Prosjektstatus)prosjekt.Prosjektstatus,
            Tilstand             = prosjekt.BTilstand != null 
                ? (Tilstand?)prosjekt.BTilstand.TilstandLokalitet
                : null,
            ProsjektIdSe         = prosjekt.ProsjektIdSe
        };
    }
    
    public async Task<string> GenerateAndSetProsjektIdSeAsync(Guid prosjektId, DateTime feltdato)
    {
        var prosjekt = await _context.BProsjekts.FindAsync(prosjektId)
                       ?? throw new InvalidOperationException("Prosjekt ikke funnet");

        // Hvis allerede satt, returner eksisterende:
        if (!string.IsNullOrEmpty(prosjekt.ProsjektIdSe))
            return prosjekt.ProsjektIdSe;

        // Hent år (to siste siffer) fra feltdato:
        var yearSuffix = feltdato.ToString("yy");

        // Finn antall andre prosjekter for samme år som har minst én preinfo:
        var countThisYear = await _context.BPreinfos
            .Where(p => p.Feltdato.Year == feltdato.Year)
            .Select(p => p.ProsjektId)
            .Distinct()
            .CountAsync();

        // Løpenummer = antall + 1
        var løpenummer = countThisYear + 1;

        // Formater: SE-25-BU-1
        var idSe = $"SE-{yearSuffix}-BU-{løpenummer}";

        // Sett og lagre:
        prosjekt.ProsjektIdSe = idSe;
        _context.BProsjekts.Update(prosjekt);
        await _context.SaveChangesAsync();

        return idSe;
    }
    
    public async Task UpdateProjectStatusAsync(Guid projectId, Prosjektstatus newStatus, string? merknad = null)
    {
        var prosjekt = await _context.BProsjekts.FindAsync(projectId);
        if (prosjekt == null)
            throw new KeyNotFoundException($"Prosjekt {projectId} ikke funnet.");

        // Dersom vi går til Ferdig eller Deaktivert, må bruker legge inn merknad:
        if ((newStatus == Prosjektstatus.Ferdig || newStatus == Prosjektstatus.Deaktivert)
            && string.IsNullOrWhiteSpace(merknad))
        {
            throw new ArgumentException("Merknad må fylles ut når status settes til Ferdig eller Deaktivert.");
        }

        prosjekt.Prosjektstatus = (int)newStatus;

        // Når man legger til merknad, append på eksisterende med ny linje:
        if (!string.IsNullOrWhiteSpace(merknad))
        {
            prosjekt.Merknad = string.IsNullOrWhiteSpace(prosjekt.Merknad)
                ? merknad
                : $"{prosjekt.Merknad}\n{merknad}";
        }

        await _context.SaveChangesAsync();
    }
}