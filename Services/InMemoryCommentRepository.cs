using System.Collections.Concurrent;
using Portfolio.Models;

namespace Portfolio.Services;

public class InMemoryCommentRepository : ICommentRepository
{
    private readonly ConcurrentDictionary<string, List<Comment>> _comments = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _writeLock = new();

    public IReadOnlyList<Comment> GetForProject(string slug)
    {
        if (!_comments.TryGetValue(slug, out var list)) return Array.Empty<Comment>();

        lock (_writeLock)
        {
            return list.OrderByDescending(comment => comment.PostedAt).ToList();
        }
    }

    public IReadOnlyDictionary<string, int> GetCounts()
    {
        lock (_writeLock)
        {
            return _comments.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Count,
                StringComparer.OrdinalIgnoreCase);
        }
    }

    public void Add(Comment comment)
    {
        lock (_writeLock)
        {
            var list = _comments.GetOrAdd(comment.ProjectSlug, _ => new List<Comment>());
            list.Add(comment);
        }
    }

    public bool Delete(string slug, Guid id, string requestedBy)
    {
        lock (_writeLock)
        {
            if (!_comments.TryGetValue(slug, out var list)) return false;

            var comment = list.FirstOrDefault(item => item.Id == id);
            if (comment is null) return false;

            if (!string.Equals(comment.PostedBy, requestedBy, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            list.Remove(comment);
            return true;
        }
    }
}
