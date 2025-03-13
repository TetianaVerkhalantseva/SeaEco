using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace SeaEco.Services.HashService;

public static class Hasher
{
    private const int ITERATIONS = 10_000;

    public static (string hashed, byte[] salt) Hash(string original)
    {
        byte[] salt = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        return Hash(original, salt);
    }

    public static bool Verify(string hashed, string original, byte[] salt)
    {
        var result = Hash(original, salt);
        return hashed == result.hashed;
    }

    public static (string hashed, byte[] salt) Hash(string original, byte[] salt)
    {
        byte[] hash = KeyDerivation.Pbkdf2(original, salt, KeyDerivationPrf.HMACSHA256, ITERATIONS, 64);
        string hashed = Convert.ToBase64String(hash);

        return (hashed, salt);
    }
}