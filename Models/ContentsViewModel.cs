namespace Portfolio.Models;

public class ContentsViewModel
{
    public IReadOnlyList<TermGroup> Terms { get; init; } = Array.Empty<TermGroup>();
    public IReadOnlyDictionary<string, int> CommentCounts { get; init; } = new Dictionary<string, int>();
    public IReadOnlyList<TermTab> Tabs { get; init; } = Array.Empty<TermTab>();
    public string? Query { get; init; }
    public string ActiveTerm { get; init; } = string.Empty;
    public int TotalProjects { get; init; }
}

public class TermTab
{
    public string Term { get; init; } = string.Empty;
    public int Count { get; init; }
}

public class TermGroup
{
    public string Term { get; init; } = string.Empty;
    public string Blurb { get; init; } = string.Empty;
    public IReadOnlyList<Project> Projects { get; init; } = Array.Empty<Project>();
}
