using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.Lokalitet;
using SeaEco.EntityFramework.Contexts;

namespace SeaEco.Services.LokalitetServices;

public class LokalitetService : ILokalitetService
{
    private readonly AppDbContext _context;
    
    public LokalitetService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<LokalitetDto>> GetAllAsync()
    {
        return await _context.Lokalitets
            .Select(l => new LokalitetDto {
                Id = l.Id,
                Navn = l.Lokalitetsnavn,
                LokalitetKode = l.LokalitetsId
            })
            .ToListAsync();
    }
}
