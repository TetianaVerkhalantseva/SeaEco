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
}