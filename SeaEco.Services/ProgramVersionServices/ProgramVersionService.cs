using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.ProgramVersion;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;

namespace SeaEco.Services.ProgramVersionServices;

public class ProgramVersionService(IGenericRepository<Programversjon> repository) : IProgramVersionService
{
    public async Task<Response<ProgramVersionDto>> CurrentVersion()
    {
        Programversjon? dbRecord = await repository.GetAll()
            .OrderByDescending(_ => _.Utgivelsesdato)
            .FirstOrDefaultAsync();

        if (dbRecord is null)
        {
            return Response<ProgramVersionDto>.Error("Version not found");
        }

        return Response<ProgramVersionDto>.Ok(new ProgramVersionDto()
        {
            Versjonsnummer = dbRecord.Versjonsnummer,
            Forbedringer = dbRecord.Forbedringer,
            Utgivelsesdato = dbRecord.Utgivelsesdato
        });
    }

    public async Task<Response<ProgramVersionDto>> GetVersion(string version)
    {
        Programversjon? dbRecord = await repository.GetBy(_ => _.Versjonsnummer == version);
        if (dbRecord is null)
        {
            return Response<ProgramVersionDto>.Error("Version not found");
        }

        return Response<ProgramVersionDto>.Ok(new ProgramVersionDto()
        {
            Versjonsnummer = dbRecord.Versjonsnummer,
            Forbedringer = dbRecord.Forbedringer,
            Utgivelsesdato = dbRecord.Utgivelsesdato
        });
    }

    public async Task<IEnumerable<ProgramVersionDto>> GetVersions()
    {
        List<Programversjon> dbRecords = await repository.GetAll().ToListAsync();
        return dbRecords.Select(_ => new ProgramVersionDto()
        {
            Versjonsnummer = _.Versjonsnummer,
            Forbedringer = _.Forbedringer,
            Utgivelsesdato = _.Utgivelsesdato
        });
    }
}