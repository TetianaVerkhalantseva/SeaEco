using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.BSurvey;
using SeaEco.Abstractions.Models.Stations;
using SeaEco.Abstractions.ValueObjects;
using SeaEco.EntityFramework.Contexts;
using SeaEco.EntityFramework.Entities;

namespace SeaEco.Services.StationServices;

public class StationService : IStationService
{
    private readonly AppDbContext _db;

    public StationService(AppDbContext db)
    {
        _db = db;
    }
    
    private async Task<int> GetNextStationNumberAsync(Guid prosjektId)
    {
        var max = await _db.BStasjons
            .Where(s => s.ProsjektId == prosjektId)
            .MaxAsync(s => (int?)s.Nummer) ?? 0;

        return max + 1;
    }

    public async Task<StationResult> GetStationsByProvetakningsplanIdAsync(Guid samplingPlanId)
    {
        var stations = await _db.BStasjons
            .Where(s => s.ProvetakingsplanId == samplingPlanId)
            .Select(s => new StationDto
            {
                Id = s.Id,
                Nummer = s.Nummer,
                UndersokelseId = s.UndersokelseId,
                KoordinatNord = s.KoordinatNord,
                KoordinatOst = s.KoordinatOst,
                Dybde = s.Dybde,
                Analyser = s.Analyser
            })
            .ToListAsync();

        return new StationResult
        {
            IsSuccess = stations.Any(),
            Message = stations.Any() ? null : "Ingen stasjoner funnet for denne prøvetakingsplanen.",
            Stations = stations
        };
    }
    
    public async Task<StationResult> GetStationsByProjectIdAsync(Guid projectId)
    {
        var stations = await _db.BStasjons
            .Where(s => s.ProsjektId == projectId)
            .Select(s => new StationDto
            {
                Id = s.Id,
                ProsjektId = s.ProsjektId,
                Nummer = s.Nummer,
                KoordinatNord = s.KoordinatNord,
                KoordinatOst = s.KoordinatOst,
                Dybde = s.Dybde,
                Analyser = s.Analyser,
                UndersokelseId = s.UndersokelseId
            })
            .ToListAsync();

        return new StationResult
        {
            IsSuccess = stations.Any(),
            Message   = stations.Any() 
                ? null 
                : "Ingen stasjoner funnet for dette prosjektet.",
            Stations  = stations
        };
    }
    
    public async Task<StationResult> GetStationByIdAsync(Guid projectId, Guid stationId)
    {
        var s = await _db.BStasjons.FirstOrDefaultAsync(s => s.ProsjektId == projectId && s.Id == stationId);
        if (s == null)
        {
            return new StationResult
            {
                IsSuccess = false,
                Message = "Stasjon ikke funnet."
            };
        }

        return new StationResult
        {
            IsSuccess = true,
            Station = new StationDto
            {
                Id = s.Id,
                Nummer = s.Nummer,
                KoordinatNord = s.KoordinatNord,
                KoordinatOst = s.KoordinatOst,
                Dybde = s.Dybde,
                UndersokelseId = s.UndersokelseId,
                Analyser = s.Analyser
            }
        };
    }

    public async Task<BStationDto?> GetBStationDtoByStationId(Guid projectId, Guid stationId)
    {
        var station = await _db.BStasjons.FirstOrDefaultAsync(s => s.ProsjektId == projectId && s.Id == stationId);
        if (station == null)
        {
            return null;
        }

        return new BStationDto
        {
            Id = station.Id,
            ProsjektId = station.ProsjektId,
            Nummer = station.Nummer,
            KoordinatNord = station.KoordinatNord,
            KoordinatOst = station.KoordinatOst,
            Dybde = station.Dybde,
            Analyser = station.Analyser
        };
    }

    public async Task<StationResult> AddStationToPlanAsync(Guid samplingPlanId, NewStationDto dto)
    {
        var plan = await _db.BProvetakningsplans
            .FirstOrDefaultAsync(p => p.ProsjektId == dto.ProsjektId && p.Id == samplingPlanId);

        if (plan == null)
        {
            return new StationResult
            {
                IsSuccess = false,
                Message = "Prøvetakningsplan finnes ikke for prosjektet."
            };
        }

        var nummer = await GetNextStationNumberAsync(dto.ProsjektId);

        Coordinate coordinate = new Coordinate(dto.NorthDegree, dto.NorthMinutes, dto.EastDegree, dto.EastMinutes);
        
        var stasjon = new BStasjon
        {
            Id = Guid.NewGuid(),
            ProsjektId = dto.ProsjektId,
            ProvetakingsplanId = samplingPlanId,
            Nummer = nummer,
            KoordinatNord = coordinate.North,
            KoordinatOst = coordinate.East,
            Dybde = dto.Dybde,
            Analyser = dto.Analyser,
            UndersokelseId = null
        };

        await _db.BStasjons.AddAsync(stasjon);
        await _db.SaveChangesAsync();

        return new StationResult
        {
            IsSuccess = true,
            Message = "Stasjon opprettet",
            Station = new StationDto
            {
                Id = stasjon.Id,
                Nummer = stasjon.Nummer,
                KoordinatNord = stasjon.KoordinatNord,
                KoordinatOst = stasjon.KoordinatOst,
                Dybde = stasjon.Dybde,
                Analyser = stasjon.Analyser
            }
        };
    }
    
    public async Task<StationResult> AddStationToProjectAsync(Guid projectId, NewStationDto dto)
    {
        var nummer = await GetNextStationNumberAsync(dto.ProsjektId);

        Coordinate coordinate = new Coordinate(dto.NorthDegree, dto.NorthMinutes, dto.EastDegree, dto.EastMinutes);
        
        var stasjon = new BStasjon
        {
            Id = Guid.NewGuid(),
            ProsjektId = dto.ProsjektId,
            ProvetakingsplanId = null,
            Nummer = nummer,
            KoordinatNord = coordinate.North,
            KoordinatOst = coordinate.East,
            Dybde = dto.Dybde,
            Analyser = dto.Analyser
        };

        await _db.BStasjons.AddAsync(stasjon);
        await _db.SaveChangesAsync();

        return new StationResult
        {
            IsSuccess = true,
            Message = "Stasjon lagt til i prosjekt",
            Station = new StationDto
            {
                Id = stasjon.Id,
                Nummer = stasjon.Nummer,
                KoordinatNord = stasjon.KoordinatNord,
                KoordinatOst = stasjon.KoordinatOst,
                Dybde = stasjon.Dybde,
                Analyser = stasjon.Analyser
            }
        };
    }

    public async Task<StationResult> UpdateStationAsync(Guid stationId, UpdateStationDto dto)
    {
        var stasjon = await _db.BStasjons.FirstOrDefaultAsync(s => s.Id == stationId);
        if (stasjon == null)
        {
            return new StationResult { IsSuccess = false, Message = "Stasjon ikke funnet." };
        }
        
        Coordinate coordinate = new Coordinate(dto.NorthDegree, dto.NorthMinutes, dto.EastDegree, dto.EastMinutes);

        stasjon.KoordinatNord = coordinate.North;
        stasjon.KoordinatOst = coordinate.East;
        stasjon.Dybde = dto.Dybde;
        stasjon.Analyser = dto.Analyser;

        await _db.SaveChangesAsync();

        return new StationResult { IsSuccess = true, Message = "Stasjon oppdatert." };
    }

    public async Task<StationResult> DeleteStationAsync(Guid stationId)
    {
        var station = await _db.BStasjons.FirstOrDefaultAsync(s => s.Id == stationId);
        if (station == null)
            return new StationResult { IsSuccess = false, Message = "Stasjon ikke funnet." };

        if (station.UndersokelseId != null)
            return new StationResult { IsSuccess = false, Message = "Kan ikke slette stasjon med tilknyttet undersøkelse." };

        _db.BStasjons.Remove(station);
        await _db.SaveChangesAsync();

        return new StationResult { IsSuccess = true, Message = "Stasjon slettet." };
    }
}