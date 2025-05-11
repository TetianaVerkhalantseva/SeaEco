using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SeaEco.Abstractions.Models.Image;
using SeaEco.Abstractions.ResponseService;
using SeaEco.EntityFramework.Entities;
using SeaEco.EntityFramework.GenericRepository;
using SeaEco.Services.ImageServices.Models;

namespace SeaEco.Services.ImageServices;

public sealed class ImageService : IImageService
{
    private const string CANT_SAVE_IMAGE_ERROR = "Can't save image error";
    private const string CANT_REMOVE_IMAGE_ERROR = "Can't remove image error";
    private const string IMAGE_NOT_FOUND_ERROR = "Image not found";
    private const string PROJECT_NOT_FDOUND_ERROR = "Project not found";
    
    private readonly IGenericRepository<BBilder> _imageRepository;
    private readonly IGenericRepository<BUndersokelse> _undersokelRepository;
    private readonly string _destinationPath;

    public ImageService(IGenericRepository<BBilder> imageRepository,
        IGenericRepository<BUndersokelse> undersokelRepository,
        IWebHostEnvironment hostingEnvironment)
    {
        _imageRepository = imageRepository;
        _undersokelRepository = undersokelRepository;
        _destinationPath = Path.Combine(hostingEnvironment.WebRootPath, "images");
    }
    
    public async Task<Response> AddImage(AddImageDto dto)
    {
        Guid imageId = Guid.NewGuid();
        
        BBilder? dbRecords = await _imageRepository.GetAll()
            .Include(_ => _.Undersokelse)
            .ThenInclude(_ => _.Prosjekt)
            .FirstOrDefaultAsync(_ => _.UndersokelseId == dto.UndersokelseId &&
                                      _.Silt == dto.Silt);
        
        if (dbRecords is not null)
        {
            Response removeResult = await DeleteImage(dbRecords.Id);
            if (removeResult.IsError)
            {
                return removeResult;
            }
        }
        
        BUndersokelse? undersokelse = await _undersokelRepository.GetAll()
            .Include(_ => _.Prosjekt)
            .FirstOrDefaultAsync(_ => _.Id == dto.UndersokelseId);
        
        if (undersokelse is null)
        {
            return Response.Error(PROJECT_NOT_FDOUND_ERROR);
        }
        
        Response<ImageModel> uploadResult = SaveImage(dto.Image, undersokelse.Prosjekt.ProsjektIdSe, imageId.ToString());
        if (uploadResult.IsError)
        {
            return Response.Error(uploadResult.ErrorMessage);
        }

        BBilder dbRecord = new BBilder()
        {
            Id = imageId,
            UndersokelseId = dto.UndersokelseId,
            Silt = dto.Silt,
            Extension = uploadResult.Value.Extension,
            Datogenerert = DateTime.Now
        };
        
        Response addResult = await _imageRepository.Add(dbRecord);
        if (addResult.IsError)
        {
            RemoveImage(dbRecords.Undersokelse.Prosjekt.ProsjektIdSe, uploadResult.Value.Name);
        }
        return addResult;
    }

    public async Task<Response> DeleteImage(Guid id)
    {
        BBilder? dbRecord = await _imageRepository.GetAll()
            .Include(_ => _.Undersokelse)
            .ThenInclude(_ => _.Prosjekt)
            .FirstOrDefaultAsync(_ => _.Id == id);
            
        if (dbRecord is null)
        {
            return Response.Error(IMAGE_NOT_FOUND_ERROR);
        }

        Response removeResult = RemoveImage(dbRecord.Undersokelse.Prosjekt.ProsjektIdSe, dbRecord.Id.ToString());
        if (removeResult.IsError)
        {
            return removeResult;
        }
        
        return await _imageRepository.Delete(dbRecord);
    }

    public async Task<Response<ImageDto>> GetImage(Guid id)
    {
        BBilder? dbRecord = await _imageRepository.GetAll()
            .Include(_ => _.Undersokelse)
            .ThenInclude(_ => _.Prosjekt)
            .FirstOrDefaultAsync(_ => _.Id == id);
        
        if (dbRecord is null)
        {
            return Response<ImageDto>.Error(IMAGE_NOT_FOUND_ERROR);
        }

        string projectIdSe = dbRecord.Undersokelse.Prosjekt.ProsjektIdSe;
        
        return Response<ImageDto>.Ok(new ImageDto()
        {
            Id = dbRecord.Id,
            UploadDate = dbRecord.Datogenerert,
            Silt = dbRecord.Silt,
            Path = Path.Combine("/images", projectIdSe, $"{dbRecord.Id.ToString()}.{dbRecord.Extension}"),
        });
    }
    
    private Response<ImageModel> SaveImage(IFormFile image, string projectIdSe, string newName)
    {
        if (!Directory.Exists(Path.Combine(_destinationPath, projectIdSe)))
        {
            Directory.CreateDirectory(Path.Combine(_destinationPath, projectIdSe));
        }

        string[] originalImageNameParts = image.FileName.Split('.');
        string originalFileExtension = originalImageNameParts.Last();
        string newFileName = $"{newName}.{originalFileExtension}";

        try
        {
            using (FileStream fileStream = new FileStream(Path.Combine(_destinationPath, projectIdSe,  newFileName), FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
        }
        catch (Exception ex)
        {
            return Response<ImageModel>.Error(CANT_SAVE_IMAGE_ERROR);
        }

        return Response<ImageModel>.Ok(new ImageModel()
        {
            Extension = originalFileExtension,
            Name = newName,
        });
    }

    private Response RemoveImage(string projectIdSe, string name)
    {
        if (!Directory.Exists(Path.Combine(_destinationPath, projectIdSe)))
        {
            return Response.Ok();
        }

        string[] files = Directory.GetFiles(Path.Combine(_destinationPath, projectIdSe));

        foreach (string file in files)
        {
            string fullFileName = Path.GetFileName(file);
            string fileName = fullFileName.Split('.').First();
            if (fileName == name)
            {
                try
                {
                    string filePath = Path.Combine(_destinationPath, projectIdSe, fullFileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    return Response.Error(CANT_REMOVE_IMAGE_ERROR);
                }
                return Response.Ok();
            }
        }
        return Response.Ok();
    }
}