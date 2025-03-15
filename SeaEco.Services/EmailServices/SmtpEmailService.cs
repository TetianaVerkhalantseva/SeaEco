using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SeaEco.Abstractions.ResponseService;
using SeaEco.Services.EmailServices.Models;

namespace SeaEco.Services.EmailServices;

public sealed class SmtpEmailService : IEmailService
{
    private const string UnexpectedError = "Unexpected error. The email was not sent";
    
    private readonly SmtpOptions _options;
    private readonly SmtpClient _client;

    public SmtpEmailService(IOptionsMonitor<SmtpOptions> monitor)
    {
        _options = monitor.CurrentValue;
        _client = new SmtpClient();
    }

    public async Task<Response> SendMail(EmailMessageModel model)
    {
        try
        {
            await _client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.SslOnConnect);
            await _client.AuthenticateAsync(_options.UserName, _options.Password);
            await _client.SendAsync(CreateMessage(
                model.Subject, 
                string.IsNullOrEmpty(model.SendFrom) ? _options.DisplayName : model.SendFrom,
                model.Content,
                model.BodyType,
                model.Recipients,
                model.CCRecipients,
                model.BCCRecipients));
            await _client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Response.Error(UnexpectedError);
        }
        return Response.Ok();
    }

    private MimeMessage CreateMessage(string subject, string? displayName, string content, EmailBodyType bodyType, IEnumerable<string> recipients, IEnumerable<string> CCRecipients, IEnumerable<string> BCCRecipients)
    {
        MimeMessage message = new MimeMessage();

        message.From.Add(new MailboxAddress(displayName, _options.Email));
        message.To.AddRange(recipients.Select(MailboxAddress.Parse));
        message.Cc.AddRange(CCRecipients.Select(MailboxAddress.Parse));
        message.Bcc.AddRange(BCCRecipients.Select(MailboxAddress.Parse));

        BodyBuilder builder = new BodyBuilder();
        if (bodyType == EmailBodyType.Html)
            builder.HtmlBody = content;
        else
            builder.TextBody = content;

        message.Body = builder.ToMessageBody();
        message.Subject = subject;

        return message;
    }
}