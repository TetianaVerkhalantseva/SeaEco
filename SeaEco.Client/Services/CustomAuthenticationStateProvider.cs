using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;
    

    public CustomAuthenticationStateProvider(HttpClient httpClient, ILogger<CustomAuthenticationStateProvider> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var test = await _httpClient.GetAsync("https://localhost:7096/" + "api/authentication/isAuthenticated");
            _logger.LogInformation(await test.Content.ReadAsStringAsync());
            var authStatus = await _httpClient.GetFromJsonAsync<AuthStatus>("https://localhost:7096/" + "api/authentication/isAuthenticated");
            _logger.LogInformation($"User authentication status: {authStatus?.IsAuthenticated}");

            if (authStatus != null && authStatus.IsAuthenticated)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "AuthenticatedUser")
                };

                var identity = new ClaimsIdentity(claims, "serverAuth");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error retrieving authentication state.");
        }
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}


public class AuthStatus
{
    public bool IsAuthenticated { get; set; }
}