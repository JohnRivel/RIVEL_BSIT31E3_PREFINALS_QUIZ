using Portfolio.Models;

namespace Portfolio.Services;

public interface ICommentRepository
{
    IReadOnlyList<Comment> GetForProject(string slug);
    IReadOnlyDictionary<string, int> GetCounts();
    void Add(Comment comment);
    bool Delete(string slug, Guid id, string requestedBy);
}
