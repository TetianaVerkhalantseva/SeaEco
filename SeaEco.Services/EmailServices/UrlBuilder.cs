namespace SeaEco.Services.EmailServices;

public static class UrlBuilder
{
    public static string ResetPasswordUrl(string token) => $"https://localhost:7095/resetPassword?token={token}";
}
