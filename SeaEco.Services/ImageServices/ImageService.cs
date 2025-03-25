using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    
    private readonly IGenericRepository<BBilder> _imageRepository;
    private readonly string _destinationPath;

    public ImageService(IGenericRepository<BBilder> imageRepository,
        IWebHostEnvironment hostingEnvironment)
    {
        _imageRepository = imageRepository;
        _destinationPath = Path.Combine(hostingEnvironment.WebRootPath, "images");
    }
    
    public async Task<Response> AddImage(AddImageDto dto)
    {
        Guid imageId = Guid.NewGuid();
        
        Response<ImageModel> uploadResult = SaveImage(dto.Image, imageId.ToString());
        if (uploadResult.IsError)
        {
            return Response.Error(uploadResult.ErrorMessage);
        }

        BBilder dbRecord = new BBilder()
        {
            Extension = uploadResult.Value.Extension,
            Silt = dto.Silt,
            Prosjektid = dto.Prosjektid,
            Datoregistrert = DateTime.Now,
            Id = imageId,
            Posisjon = dto.Posisjon,
            Stasjonsid = dto.Stasjonsid
        };
        
        Response addResult = await _imageRepository.Add(dbRecord);
        if (addResult.IsError)
        {
            return RemoveImage(uploadResult.Value.Name);
        }

        return addResult;
    }

    public async Task<Response> DeleteImage(Guid id)
    {
        BBilder? dbRecord = await _imageRepository.GetBy(record => record.Id == id);
        if (dbRecord is null)
        {
            return Response.Error(IMAGE_NOT_FOUND_ERROR);
        }

        return RemoveImage(dbRecord.Id.ToString());
    }
    
    private Response<ImageModel> SaveImage(IFormFile image, string newName)
        {
            if (!Directory.Exists(_destinationPath))
            {
                Directory.CreateDirectory(_destinationPath);
            }

            string[] originalImageNameParts = image.FileName.Split('.');
            string originalFileExtension = originalImageNameParts.Last();
            string newFileName = $"{newName}.{originalFileExtension}";

            try
            {
                using (FileStream fileStream = new FileStream(Path.Combine(_destinationPath, newFileName), FileMode.Create))
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

        private Response RemoveImage(string name)
        {
            if (!Directory.Exists(_destinationPath))
            {
                return Response.Ok();
            }

            string[] files = Directory.GetFiles(_destinationPath);

            foreach (string file in files)
            {
                string fileName = file.Split('.').First();
                if (fileName == name)
                {
                    try
                    {
                        File.Delete(Path.Combine(_destinationPath, file));
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

        private Response RemoveImage(string name, string extension)
        {
            if (!Directory.Exists(_destinationPath))
            {
                return Response.Ok();
            }

            string[] files = Directory.GetFiles(_destinationPath);

            foreach (string file in files)
            {
                string[] fileNameParts = file.Split('.');
                string fileName = fileNameParts.First();
                string fileExtension = fileNameParts.Last();

                if (fileName == name && fileExtension == extension)
                {
                    try
                    {
                        File.Delete(Path.Combine(_destinationPath, file));
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