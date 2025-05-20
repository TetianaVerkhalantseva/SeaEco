namespace SeaEco.Services.JwtServices;

public sealed record JwtOptions
{
    public string Key { get; set; } = string.Empty;
    public int Expiration { get; set; }
}
