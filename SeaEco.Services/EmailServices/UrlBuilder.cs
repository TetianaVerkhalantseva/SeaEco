namespace SeaEco.Services.EmailServices;

public static class UrlBuilder
{
    public static string RestorePasswordUrl(string token) => $"/api/authentication/restore-password?token={token}";
}