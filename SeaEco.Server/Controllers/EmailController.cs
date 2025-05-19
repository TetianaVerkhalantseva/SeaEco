using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Services.EmailServices;
using SeaEco.Services.EmailServices.Models;

namespace SeaEco.Server.Controllers;

[Route("/api/email")]
public class EmailController(IEmailService emailService) : ApiControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] EmailMessageModel model)
    {
        Response response = await emailService.SendMail(model);
        return response.IsError
            ? AsBadRequest(response.ErrorMessage)
            : AsOk();
    }
}
