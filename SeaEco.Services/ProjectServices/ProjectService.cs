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
        
        var customer = await _context.Kundes.FindAsync(dto.KundeId);
        if (customer == null)
            throw new KeyNotFoundException("Kunde ikke funnet.");
        
        var prosjekt = new BProsjekt
        {
            PoId = dto.PoId,
            KundeId = dto.KundeId,
            Kundekontaktperson = dto.Kundekontaktperson,
            Kundetlf = dto.Kundetlf,
            Kundeepost = dto.Kundeepost,
            //LokalitetId = dto.Lokalitetid, //Se hvordan lokalitet endret. Dette er Fk til Lokalitet tabell - der lokalitetsnavn oh lokalitetsID
            Mtbtillatelse = dto.Mtbtillatelse,
            ProsjektansvarligId = dto.ProsjektansvarligId,
            Merknad = dto.Merknad,
            Produksjonsstatus = (int)dto.Produksjonsstatus,
        };

        _context.BProsjekts.Add(prosjekt);
        await _context.SaveChangesAsync();

        return prosjekt.Id;
    }
    
    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        return await _context.BProsjekts
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                PoId = p.PoId,
                KundeId = p.KundeId,
                Kundekontaktperson = p.Kundekontaktperson,
                Kundetlf = p.Kundetlf,
                Kundeepost = p.Kundeepost,
                //Lokalitetid = p.LokalitetId, //Se hvordan lokalitet endret. Dette er Fk til Lokalitet tabell - der lokalitetsnavn oh lokalitetsID
                Mtbtillatelse = p.Mtbtillatelse,
                Merknad = p.Merknad,
                Produksjonsstatus = (Produksjonsstatus)p.Produksjonsstatus,
            })
            .ToListAsync();
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(Guid Id)
    {
        var p = await _context.BProsjekts.FirstOrDefaultAsync(p => p.Id == Id);
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
            //Lokalitetid = p.LokalitetId, //Se hvordan lokalitet endret. Dette er Fk til Lokalitet tabell - der lokalitetsnavn oh lokalitetsID
            Mtbtillatelse = p.Mtbtillatelse,
            Merknad = p.Merknad,
            Produksjonsstatus = (Produksjonsstatus)p.Produksjonsstatus,
        };
    }
}