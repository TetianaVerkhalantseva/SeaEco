namespace SeaEco.Services.EmailServices;

public sealed class EmailMessageManager
{
    private const string TEMPLATE_FOLDER_PATH = "wwwroot/templates/";
    private const string RESET_PASSWORD_TEMPLATE = "ResetPasswordTemplate.html";

    public async Task<string> ResetPasswordTemplate(string url)
    {
        using StreamReader reader = new StreamReader($"{TEMPLATE_FOLDER_PATH}{RESET_PASSWORD_TEMPLATE}");
        string html = await reader.ReadToEndAsync();
        return html.Replace("{0}", url);
    }

}