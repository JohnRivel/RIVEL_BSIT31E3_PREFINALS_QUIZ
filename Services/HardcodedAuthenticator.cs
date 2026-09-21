using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace Portfolio.Services;

public class HardcodedAuthenticator : IPortfolioAuthenticator
{
    private const int Iterations = 210_000;
    private const int MaxAttempts = 5;
    private static readonly TimeSpan LockoutWindow = TimeSpan.FromMinutes(5);

    private readonly string _username;
    private readonly string _displayName;
    private readonly string _publishedPassword;
    private readonly byte[] _salt;
    private readonly byte[] _hash;
    private readonly ConcurrentDictionary<string, (int Count, DateTimeOffset First)> _attempts = new();

    public HardcodedAuthenticator(IConfiguration configuration)
    {
        var section = configuration.GetSection("PortfolioLogin");
        _username = section["Username"] ?? "admin";
        _displayName = section["DisplayName"] ?? "John Cristian Rivel";
        _publishedPassword = section["Password"] ?? "admin123";
        _salt = RandomNumberGenerator.GetBytes(16);
        _hash = Hash(_publishedPassword, _salt);
    }

    public (string Username, string Password) PublishedCredentials => (_username, _publishedPassword);

    public LoginResult Validate(string username, string password, string clientKey)
    {
        var now = DateTimeOffset.UtcNow;

        if (_attempts.TryGetValue(clientKey, out var state))
        {
            if (now - state.First > LockoutWindow)
            {
                _attempts.TryRemove(clientKey, out _);
            }
            else if (state.Count >= MaxAttempts)
            {
                var wait = LockoutWindow - (now - state.First);
                return new LoginResult(
                    false,
                    Error: $"Too many attempts. Try again in {Math.Ceiling(wait.TotalMinutes)} minute(s).",
                    RetryAfter: wait);
            }
        }

        var userMatches = string.Equals(username.Trim(), _username, StringComparison.OrdinalIgnoreCase);
        var passwordMatches = CryptographicOperations.FixedTimeEquals(Hash(password, _salt), _hash);

        if (userMatches && passwordMatches)
        {
            _attempts.TryRemove(clientKey, out _);
            return new LoginResult(true, _displayName);
        }

        _attempts.AddOrUpdate(
            clientKey,
            _ => (1, now),
            (_, existing) => (existing.Count + 1, existing.First));

        return new LoginResult(false, Error: "That username and password do not match.");
    }

    private static byte[] Hash(string password, byte[] salt) =>
        Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            32);
}
