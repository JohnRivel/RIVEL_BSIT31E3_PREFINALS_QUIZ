using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models;

public class Comment
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string ProjectSlug { get; init; } = string.Empty;

    [Required(ErrorMessage = "Enter your name.")]
    [StringLength(40)]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a comment.")]
    [StringLength(600)]
    public string Body { get; set; } = string.Empty;

    public DateTimeOffset PostedAt { get; init; } = DateTimeOffset.UtcNow;
    public string PostedBy { get; init; } = string.Empty;
}
