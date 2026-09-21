using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio.Controllers;

[Authorize]
public class ProjectsController : Controller
{
    private readonly IProjectRepository _projects;
    private readonly ICommentRepository _comments;

    public ProjectsController(IProjectRepository projects, ICommentRepository comments)
    {
        _projects = projects;
        _comments = comments;
    }

    [HttpGet]
    public IActionResult Index(string? q = null, string? term = null)
    {
        // No term picked = the home screen with every term as its own row.
        string? activeTerm = string.IsNullOrWhiteSpace(term) ? null : _projects.ResolveTerm(term);

        return View(new ContentsViewModel
        {
            Terms = _projects.GetGrouped(q, activeTerm),
            Tabs = _projects.GetTermTabs(q),
            ActiveTerm = activeTerm ?? string.Empty,
            CommentCounts = _comments.GetCounts(),
            Query = q,
            TotalProjects = _projects.GetAll().Count
        });
    }

    [HttpGet]
    public IActionResult Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return RedirectToAction(nameof(Index));

        var project = _projects.GetBySlug(id);
        if (project is null) return NotFound();

        var neighbours = _projects.GetNeighbours(project.Slug);

        return View(new ProjectDetailViewModel
        {
            Project = project,
            Comments = _comments.GetForProject(project.Slug),
            NewComment = new Comment { ProjectSlug = project.Slug },
            Previous = neighbours.Previous,
            Next = neighbours.Next
        });
    }

    [HttpPost]
    public IActionResult Comment(string id, [Bind(Prefix = "NewComment")] Comment newComment)
    {
        var project = _projects.GetBySlug(id);
        if (project is null) return NotFound();

        ModelState.Remove("NewComment.ProjectSlug");

        if (!ModelState.IsValid)
        {
            var neighbours = _projects.GetNeighbours(project.Slug);
            return View("Details", new ProjectDetailViewModel
            {
                Project = project,
                Comments = _comments.GetForProject(project.Slug),
                NewComment = newComment,
                Previous = neighbours.Previous,
                Next = neighbours.Next
            });
        }

        _comments.Add(new Comment
        {
            ProjectSlug = project.Slug,
            Author = Clean(newComment.Author, 40),
            Body = Clean(newComment.Body, 600),
            PostedBy = User.FindFirstValue(ClaimTypes.Name) ?? "unknown"
        });

        TempData["CommentPosted"] = "Comment posted.";
        return RedirectToAction(nameof(Details), new { id = project.Slug });
    }

    [HttpPost]
    public IActionResult DeleteComment(string id, Guid commentId)
    {
        var project = _projects.GetBySlug(id);
        if (project is null) return NotFound();

        var user = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        TempData["CommentPosted"] = _comments.Delete(project.Slug, commentId, user)
            ? "Comment deleted."
            : "You can only delete comments you posted.";

        return RedirectToAction(nameof(Details), new { id = project.Slug });
    }

    private static string Clean(string? input, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var cleaned = new string(input.Where(c => !char.IsControl(c) || c == '\n').ToArray()).Trim();
        return cleaned.Length <= maxLength ? cleaned : cleaned[..maxLength];
    }
}
