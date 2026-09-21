namespace Portfolio.Models;

public class ProjectDetailViewModel
{
    public Project Project { get; init; } = new();
    public IReadOnlyList<Comment> Comments { get; init; } = Array.Empty<Comment>();
    public Comment NewComment { get; init; } = new();
    public Project? Previous { get; init; }
    public Project? Next { get; init; }
}
