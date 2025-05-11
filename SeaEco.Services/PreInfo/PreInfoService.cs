using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Enums;
using SeaEco.Abstractions.Models.PreInfo;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;
using SeaEco.Services.ProjectServices;

namespace SeaEco.Services.PreInfo;

public class PreInfoService : IPreInfoService
{
    private readonly AppDbContext _db;
    private readonly IProjectService _projectService;

    public PreInfoService(AppDbContext db, IProjectService projectService)
    {
        _db = db;
        _projectService = projectService;
    }
    
    public async Task<PreInfoDto?> GetByIdAsync(Guid preInfoId)
    {
        var p = await _db.BPreinfos
            .Include(x => x.Provetakers)
            .FirstOrDefaultAsync(x => x.Id == preInfoId);

        if (p == null) return null;

        return new PreInfoDto
        {
            Id               = p.Id,
            ProsjektId       = p.ProsjektId,
            Feltdato         = p.Feltdato,
            FeltansvarligId  = p.FeltansvarligId,
            ProvetakerIds    = p.Provetakers.Select(u => u.Id).ToList(),
            
            Ph               = p.PhSjo,
            Eh               = p.EhSjo,
            Temperatur       = p.SjoTemperatur,
            RefElektrode     = p.RefElektrode,

            Grabb            = p.Grabb,
            Sil              = p.Sil,
            PhMeter          = p.PhMeter,
            Kalibreringsdato = p.Kalibreringsdato
        };
    }

    public async Task<List<PreInfoDto>> GetAllByProjectAsync(Guid prosjektId)
    {
        var list = await _db.BPreinfos
            .Where(x => x.ProsjektId == prosjektId)
            .Include(x => x.Provetakers)
            .ToListAsync();

        return list.Select(p => new PreInfoDto
        {
            Id               = p.Id,
            ProsjektId       = p.ProsjektId,
            Feltdato         = p.Feltdato,
            FeltansvarligId  = p.FeltansvarligId,
            ProvetakerIds    = p.Provetakers.Select(u => u.Id).ToList(),
            
            Ph               = p.PhSjo,
            Eh               = p.EhSjo,
            Temperatur       = p.SjoTemperatur,
            RefElektrode     = p.RefElektrode,

            Grabb            = p.Grabb,
            Sil              = p.Sil,
            PhMeter          = p.PhMeter,
            Kalibreringsdato = p.Kalibreringsdato
        }).ToList();
    }
    

    public async Task<Guid> CreatePreInfoAsync(AddPreInfoDto dto)
    {
        bool exists = await _db.BPreinfos
            .AnyAsync(x =>
                x.ProsjektId == dto.ProsjektId
                && x.Feltdato.Date == dto.Feltdato.Date);
        if (exists)
        {
            throw new InvalidOperationException(
                $"Det finnes allerede en PreInfo for prosjekt {dto.ProsjektId} på dato {dto.Feltdato:yyyy-MM-dd}.");
        }
        
        var prosjekt = await _db.BProsjekts
            .FirstOrDefaultAsync(p => p.Id == dto.ProsjektId);
        if (prosjekt == null)
            throw new KeyNotFoundException($"Prosjekt {dto.ProsjektId} ikke funnet.");
        
        // Generer ProsjektIdSe dersom nødvendig
        await _projectService.GenerateAndSetProsjektIdSeAsync(dto.ProsjektId, dto.Feltdato);
        
        prosjekt.Prosjektstatus = (int)Prosjektstatus.Pabegynt;

        var entity = new BPreinfo
        {
            Id               = Guid.NewGuid(),
            ProsjektId       = dto.ProsjektId,
            Feltdato         = dto.Feltdato,
            FeltansvarligId  = dto.FeltansvarligId,

            PhSjo               = dto.Ph,
            EhSjo               = dto.Eh,
            SjoTemperatur       = dto.Temperatur,
            RefElektrode     = dto.RefElektrode,

            Grabb            = dto.Grabb,
            Sil              = dto.Sil,
            PhMeter          = dto.PhMeter,
            Kalibreringsdato = dto.Kalibreringsdato
        };

        // Knytt prøvetakere
        if (dto.ProvetakerIds?.Any() == true)
        {
            var users = await _db.Brukers
                .Where(u => dto.ProvetakerIds.Contains(u.Id))
                .ToListAsync();
            foreach (var u in users)
                entity.Provetakers.Add(u);
        }

        _db.BPreinfos.Add(entity);
        await _db.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<PreInfoDto?> EditPreInfoAsync(Guid preInfoId, EditPreInfoDto dto)
    {
        var existing = await _db.BPreinfos
            .Include(x => x.Provetakers)
            .FirstOrDefaultAsync(x => x.Id == preInfoId);

        if (existing == null)
            return null;

        existing.Feltdato         = dto.Feltdato;
        existing.FeltansvarligId  = dto.FeltansvarligId;

        existing.PhSjo               = dto.Ph;
        existing.EhSjo               = dto.Eh;
        existing.SjoTemperatur       = dto.Temperatur;
        existing.RefElektrode     = dto.RefElektrode;

        existing.Grabb            = dto.Grabb;
        existing.Sil              = dto.Sil;
        existing.PhMeter          = dto.PhMeter;
        existing.Kalibreringsdato = dto.Kalibreringsdato;

        // Oppdater prøvetakere
        existing.Provetakers.Clear();
        if (dto.ProvetakerIds?.Any() == true)
        {
            var users = await _db.Brukers
                .Where(u => dto.ProvetakerIds.Contains(u.Id))
                .ToListAsync();
            foreach (var u in users)
                existing.Provetakers.Add(u);
        }

        await _db.SaveChangesAsync();
        return new PreInfoDto
        {
            Id               = existing.Id,
            Feltdato         = existing.Feltdato,
            FeltansvarligId  = existing.FeltansvarligId,
            ProvetakerIds    = existing.Provetakers.Select(u => u.Id).ToList(),
            Ph               = existing.PhSjo,
            Eh               = existing.EhSjo,
            Temperatur       = existing.SjoTemperatur,
            RefElektrode     = existing.RefElektrode,
            Grabb            = existing.Grabb,
            Sil              = existing.Sil,
            PhMeter          = existing.PhMeter,
            Kalibreringsdato = existing.Kalibreringsdato
        };
    }

    public async Task<bool> DeletePreInfoAsync(Guid preInfoId)
    {
        var p = await _db.BPreinfos.FindAsync(preInfoId);
        if (p == null) 
            return false;

        _db.BPreinfos.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }
}