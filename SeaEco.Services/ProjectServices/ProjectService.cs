using Microsoft.EntityFrameworkCore;
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
        
        var customer = await _context.Kundes.FindAsync(dto.KundeId);
        if (customer == null)
            throw new KeyNotFoundException("Kunde ikke funnet.");
        
        var prosjekt = new BProsjekt
        {
            PoId = dto.PoId,
            Kundeid = dto.KundeId,
            Kundekontaktpersons = dto.Kundekontaktpersons,
            Kundetlf = dto.Kundetlf,
            Kundeepost = dto.Kundeepost,
            Lokalitetid = dto.Lokalitetid,
            Lokalitet = dto.Lokalitet,
            Antallstasjoner = dto.Antallstasjoner,
            Mtbtillatelse = dto.Mtbtillatelse,
            Biomasse = dto.Biomasse,
            Ansvarligansattid = dto.Ansvarligansattid,
            Ansvarligansatt2id = dto.Ansvarligansatt2id,
            Ansvarligansatt3id = dto.Ansvarligansatt3id,
            Ansvarligansatt4id = dto.Ansvarligansatt4id,
            Ansvarligansatt5id = dto.Ansvarligansatt5id,
            Planlagtfeltdato = dto.Planlagtfeltdato,
            Merknad = dto.Merknad,
            Status = dto.Status,
            Datoregistrert = DateTime.UtcNow
        };

        _context.BProsjekts.Add(prosjekt);
        await _context.SaveChangesAsync();

        return prosjekt.Prosjektid;
    }
    
    public async Task<List<ProjectDto>> GetProjectsByCustomerAsync(int kundeId)
    {
        return await _context.BProsjekts
            .Where(p => p.Kundeid == kundeId)
            .Select(p => new ProjectDto
            {
                ProsjektId = p.Prosjektid,
                PoId = p.PoId,
                KundeId = p.Kundeid,
                Kundekontaktpersons = p.Kundekontaktpersons,
                Kundetlf = p.Kundetlf,
                Kundeepost = p.Kundeepost,
                Lokalitetid = p.Lokalitetid,
                Lokalitet = p.Lokalitet,
                Antallstasjoner = p.Antallstasjoner,
                Mtbtillatelse = p.Mtbtillatelse,
                Biomasse = p.Biomasse,
                Planlagtfeltdato = p.Planlagtfeltdato,
                Merknad = p.Merknad,
                Status = p.Status,
                Datoregistrert = p.Datoregistrert
            })
            .ToListAsync();
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(Guid prosjektId)
    {
        var p = await _context.BProsjekts.FirstOrDefaultAsync(p => p.Prosjektid == prosjektId);
        if (p == null)
            return null;

        return new ProjectDto
        {
            ProsjektId = p.Prosjektid,
            PoId = p.PoId,
            KundeId = p.Kundeid,
            Kundekontaktpersons = p.Kundekontaktpersons,
            Kundetlf = p.Kundetlf,
            Kundeepost = p.Kundeepost,
            Lokalitetid = p.Lokalitetid,
            Lokalitet = p.Lokalitet,
            Antallstasjoner = p.Antallstasjoner,
            Mtbtillatelse = p.Mtbtillatelse,
            Biomasse = p.Biomasse,
            Planlagtfeltdato = p.Planlagtfeltdato,
            Merknad = p.Merknad,
            Status = p.Status,
            Datoregistrert = p.Datoregistrert
        };
    }
}