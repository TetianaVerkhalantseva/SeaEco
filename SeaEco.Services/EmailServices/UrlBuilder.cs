namespace SeaEco.Services.EmailServices;

public static class UrlBuilder
{
    public static string ResetPasswordUrl(string token) => $"/api/authentication/reset-password?token={token}";
}