namespace SeaEco.Services.EmailServices.Models;

public sealed class EmailMessageModel
{
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string SendFrom { get; set; } = string.Empty;

    public EmailBodyType BodyType { get; set; }

    public IEnumerable<string> Recipients { get; set; } = [];
    public IEnumerable<string> CCRecipients { get; set; } = [];
    public IEnumerable<string> BCCRecipients { get; set; } = [];
}