using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Services.ImageServices;
using SeaEco.Services.ImageServices.Models;

namespace SeaEco.Server.Controllers;

[Route("/api/images")]
public class ImageController(IImageService imageService) : ApiControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] AddImageDto dto)
    {
        Response response = await imageService.AddImage(dto);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }

    [HttpPut("{id:guid}/remove")]
    public async Task<IActionResult> RemoveImage([FromRoute] Guid id)
    {
        Response response = await imageService.DeleteImage(id);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }
}