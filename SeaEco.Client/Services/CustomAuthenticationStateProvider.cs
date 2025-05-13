using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;


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
            var authStatus = await _httpClient.GetFromJsonAsync<AuthStatus>( "api/authentication/isAuthenticated");
            
            Console.WriteLine($@"Role from authStatus: {authStatus.Roles}");
            
            _logger.LogInformation("Role from authStatus: {Roles}", string.Join(", ", authStatus.Roles));
            
            if (authStatus != null && authStatus.IsAuthenticated)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "AuthenticatedUser"),    
                };
                
                foreach (var role in authStatus.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                    
                }
                
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
    public List<string> Roles { get; set; }
}