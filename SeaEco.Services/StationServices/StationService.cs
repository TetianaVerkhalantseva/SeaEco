using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.Stations;
using SeaEco.EntityFramework.Contexts;

namespace SeaEco.Services.StationServices;

public class StationService : IStationService
{
    
    private readonly AppDbContext _context;

    public StationService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<StationDto>> GetStationsAsync(Guid prosjektId)
    {
        return await _context.BStasjons
            .Where(s => s.Prosjektid == prosjektId)
            .Select(s => new StationDto
            {
                ProsjektId = s.Prosjektid,
                Stasjonsid = s.Stasjonsid,
                Datoregistrert = s.Datoregistrert,
                Dybde = s.Dybde,
                Kordinatern = s.Kordinatern,
                Kordinatero = s.Kordinatero,
                SkjovannPh = s.SkjovannPh,
                SkjovannEh = s.SkjovannEh,
                SkjovannTemperatur = s.SkjovannTemperatur,
                Bunntype = s.Bunntype,
                Dyr = s.Dyr,
                Antallgrabbskudd = s.Antallgrabbskudd,
                Grabhastighetgodkjent = s.Grabhastighetgodkjent,
                Sensoriskutfort = s.Sensoriskutfort,
                Bunnsammensettningid = s.Bunnsammensettningid,
                Arter = s.Arter,
                Merknad = s.Merknad,
                Korrigering = s.Korrigering,
                Grabbid = s.Grabbid,
                Phehmeter = s.Phehmeter,
                Datokalibrert = s.Datokalibrert,
                Silid = s.Silid,
                Status = s.Status
            })
            .ToListAsync();
    }

    public async Task<StationDto?> GetStationByIdAsync(Guid prosjektId, int stasjonsid)
    {
        var s = await _context.BStasjons
            .FirstOrDefaultAsync(s => s.Prosjektid == prosjektId && s.Stasjonsid == stasjonsid);
        if (s == null)
            return null;
        return new StationDto
        {
            ProsjektId = s.Prosjektid,
            Stasjonsid = s.Stasjonsid,
            Datoregistrert = s.Datoregistrert,
            Dybde = s.Dybde,
            Kordinatern = s.Kordinatern,
            Kordinatero = s.Kordinatero,
            SkjovannPh = s.SkjovannPh,
            SkjovannEh = s.SkjovannEh,
            SkjovannTemperatur = s.SkjovannTemperatur,
            Bunntype = s.Bunntype,
            Dyr = s.Dyr,
            Antallgrabbskudd = s.Antallgrabbskudd,
            Grabhastighetgodkjent = s.Grabhastighetgodkjent,
            Sensoriskutfort = s.Sensoriskutfort,
            Bunnsammensettningid = s.Bunnsammensettningid,
            Arter = s.Arter,
            Merknad = s.Merknad,
            Korrigering = s.Korrigering,
            Grabbid = s.Grabbid,
            Phehmeter = s.Phehmeter,
            Datokalibrert = s.Datokalibrert,
            Silid = s.Silid,
            Status = s.Status
        };
    }

    public async Task UpdateStationAsync(Guid prosjektId, int stasjonsid, UpdateStationDto dto)
    {
        var station = await _context.BStasjons
            .FirstOrDefaultAsync(s => s.Prosjektid == prosjektId && s.Stasjonsid == stasjonsid);
        if (station == null)
            throw new Exception("Stasjon ikke funnet.");
        
        station.Dybde = dto.Dybde;
        station.Kordinatern = dto.Kordinatern;
        station.Kordinatero = dto.Kordinatero;
        station.SkjovannPh = dto.SkjovannPh;
        station.SkjovannEh = dto.SkjovannEh;
        station.SkjovannTemperatur = dto.SkjovannTemperatur;
        station.Bunntype = dto.Bunntype;
        station.Dyr = dto.Dyr;
        station.Antallgrabbskudd = dto.Antallgrabbskudd;
        station.Grabhastighetgodkjent = dto.Grabhastighetgodkjent;
        station.Sensoriskutfort = dto.Sensoriskutfort;
        station.Bunnsammensettningid = dto.Bunnsammensettningid;
        station.Arter = dto.Arter;
        station.Merknad = dto.Merknad;
        station.Korrigering = dto.Korrigering;
        station.Grabbid = dto.Grabbid;
        station.Phehmeter = dto.Phehmeter;
        station.Datokalibrert = dto.Datokalibrert;
        station.Silid = dto.Silid;
        station.Status = dto.Status;

        _context.BStasjons.Update(station);
        await _context.SaveChangesAsync();
    }
}