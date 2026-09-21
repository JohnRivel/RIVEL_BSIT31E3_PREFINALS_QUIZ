using Portfolio.Models;

namespace Portfolio.Services;

public interface IProjectRepository
{
    IReadOnlyList<Project> GetAll();
    IReadOnlyList<TermGroup> GetGrouped(string? query = null, string? term = null);
    IReadOnlyList<TermTab> GetTermTabs(string? query = null);
    string ResolveTerm(string? term);
    Project? GetBySlug(string slug);
    (Project? Previous, Project? Next) GetNeighbours(string slug);
}
