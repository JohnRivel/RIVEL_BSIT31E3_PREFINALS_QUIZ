namespace Portfolio.Services;

public record LoginResult(
    bool Succeeded,
    string? DisplayName = null,
    string? Error = null,
    TimeSpan? RetryAfter = null);

public interface IPortfolioAuthenticator
{
    LoginResult Validate(string username, string password, string clientKey);
    (string Username, string Password) PublishedCredentials { get; }
}
