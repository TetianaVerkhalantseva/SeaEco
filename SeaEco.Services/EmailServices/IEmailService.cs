using SeaEco.Abstractions.ResponseService;
using SeaEco.Services.EmailServices.Models;

namespace SeaEco.Services.EmailServices;

public interface IEmailService
{
    Task<Response> SendMail(EmailMessageModel model);
}