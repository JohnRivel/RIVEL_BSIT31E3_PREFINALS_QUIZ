namespace Portfolio.Models;

public class Project
{
    public string Slug { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Term { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Course { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string RepositoryUrl { get; init; } = string.Empty;
    public IReadOnlyList<string> Tech { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> Highlights { get; init; } = Array.Empty<string>();

    // Web path of the thumbnail (a real screenshot if one exists, otherwise the placeholder SVG).
    public string Thumbnail { get; set; } = string.Empty;

    // Longer write-up if there is one, otherwise the short summary.
    public string Overview => string.IsNullOrWhiteSpace(Description) ? Summary : Description;

    // Clickable GitHub address without the ".git" clone suffix.
    public string RepositoryWebUrl =>
        RepositoryUrl.EndsWith(".git", StringComparison.OrdinalIgnoreCase)
            ? RepositoryUrl[..^4]
            : RepositoryUrl;

    public string RepositoryName => RepositoryWebUrl.TrimEnd('/').Split('/').Last();
}
