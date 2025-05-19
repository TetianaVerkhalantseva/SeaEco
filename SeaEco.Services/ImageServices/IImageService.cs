using SeaEco.Abstractions.Models.Image;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Services.ImageServices.Models;

namespace SeaEco.Services.ImageServices;

public interface IImageService
{
    Task<Response<ImageDto>> AddImage(AddImageDto dto);
    Task<Response> DeleteImage(Guid id);
    Task<Response<ImageDto>> GetImage(Guid id);
    Task<IEnumerable<ImageDto>> GetImagesByUndersokelse(Guid undersokelId);
}
