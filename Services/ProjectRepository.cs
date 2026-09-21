using Portfolio.Models;

namespace Portfolio.Services;

public class ProjectRepository : IProjectRepository
{
    private static readonly (string Term, string Blurb)[] TermOrder =
    {
        ("Prelim", "Prelim activities, quizzes and examination projects."),
        ("Midterm", "Midterm quizzes, hands-on activities and examination projects."),
        ("Prefinals", "Prefinal activities and examination projects."),
        ("Finals", "No finals projects have been added yet.")
    };

    private static readonly string[] DefaultTech = { "ASP.NET Core MVC", "C#", "Razor" };
    private static readonly string[] ScreenshotExtensions = { ".png", ".jpg", ".jpeg", ".webp" };

    // Placeholder thumbnail sets in wwwroot/img/thumbs/<style>/. Pick one with
    // "Portfolio:ThumbnailStyle" in appsettings.json.
    private static readonly string[] ThumbnailStyles = { "aurora", "ios-icon", "ios-widget", "neon", "classic" };
    private const string DefaultThumbnailStyle = "aurora";

    private readonly List<Project> _projects;

    public ProjectRepository(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _projects = BuildProjects();

        var configured = configuration["Portfolio:ThumbnailStyle"];
        var style = ThumbnailStyles.FirstOrDefault(s => string.Equals(s, configured, StringComparison.OrdinalIgnoreCase))
                    ?? DefaultThumbnailStyle;

        foreach (var project in _projects)
        {
            project.Thumbnail = ResolveThumbnail(environment.WebRootPath, project.Slug, style);
        }
    }

    // EDIT HERE: to give a project a proper write-up, fill in `description` and `highlights`
    // for its row below. To use a real screenshot instead of the placeholder, save it as
    // wwwroot/img/projects/<slug>.png and restart the app.
    private static List<Project> BuildProjects() => new()
    {
        // ---- Prelim ----
        Make("prelim-q1", "Prelim Quiz 1", "Prelim",
            "Prelim Quiz 1 project.",
            "https://github.com/JohnRivel/BSIT_31E3_PRELIM_Q1_Rivel_JohnCristian.git"),
        Make("prelim-a1", "Prelim Activity 1", "Prelim",
            "Prelim activity focused on the fundamentals covered in class.",
            "https://github.com/JohnRivel/BSIT31A3_Prelim_A1_RivelJohnCristian.git"),
        Make("prelim-h1-main", "Prelim Hands-On 1", "Prelim",
            "Prelim hands-on activity demonstrating the first set of practical skills.",
            "https://github.com/JohnRivel/BSIT31E3_PRELIM_H1_Rivel_JohnCristian.git"),
        Make("prelim-h1", "Prelim Hands-On 1 (Alternate)", "Prelim",
            "Second repository for Prelim Hands-On 1.",
            "https://github.com/JohnRivel/BSIT31E3_PRELIM_H1_Rivel_JohnCristianI..git"),
        Make("prelim-h2", "Prelim Hands-On 2", "Prelim",
            "Prelim Hands-On 2 project.",
            "https://github.com/JohnRivel/BSIT31E3_PRELIM_H2_Rivel_JohnCristian.git"),
        Make("prelim-exam", "Prelim Exam", "Prelim",
            "Prelim examination project.",
            "https://github.com/JohnRivel/IT_ELECTIVE_2_PRELIM_EXAM_Rivel_JohnCristian.git"),

        // ---- Midterm ----
        Make("midterm-h1-h3", "Midterm Hands-On 1 to 3", "Midterm",
            "Collection of the Midterm Hands-On 1, 2 and 3 activities.",
            "https://github.com/JohnRivel/RIVEL_IT_ELECTIVE_2_MIDTERM_H1_H2_H3.git"),
        Make("midterm-q2-backup", "Midterm Quiz 2 Backup", "Midterm",
            "Backup repository for the Midterm Quiz 2 project.",
            "https://github.com/JohnRivel/IT_ELECTIVE_2_MIDTERM_Q2_Rivel_JohnCristian_Backup.git"),
        Make("midterm-q3", "Midterm Quiz 3", "Midterm",
            "Midterm Quiz 3 project.",
            "https://github.com/JohnRivel/Rivel_IT_ELECTIVE_2_MIDTERM_Q3.git"),
        Make("midterm-exam", "Midterm Exam", "Midterm",
            "Midterm examination project.",
            "https://github.com/JohnRivel/RIVEL_IT_ELECTIVE_2_MIDTERM_EXAM.git"),
        Make("midterm-set-5", "Midterm Exam Set 5", "Midterm",
            "Midterm examination project for Set 5.",
            "https://github.com/JohnRivel/IT_ELECTIVE_2_MIDTERM_EXAM_SET_5_RIVEL_JOHNCRISTIAN.git"),

        // ---- Prefinals ----
        Make("prefinal", "Prefinal Project", "Prefinals",
            "Prefinal project bringing together the major concepts from the course.",
            "https://github.com/JohnRivel/ITELECTIVE2_PREFINAL_RIVEL.git"),
        Make("prefinal-exam", "Prefinal Exam", "Prefinals",
            "Prefinal examination project.",
            "https://github.com/JohnRivel/IT_ELECTIVE_2_BSIT31E3_PREFINAL_EXAM_RIVEL_JOHNCRISTIAN.git"),
    };

    private static Project Make(
        string slug,
        string title,
        string term,
        string summary,
        string repositoryUrl,
        string description = "",
        string[]? tech = null,
        string[]? highlights = null) => new()
    {
        Slug = slug,
        Title = title,
        Term = term,
        Role = "Solo",
        Course = "IT Elective 2",
        Summary = summary,
        Description = description,
        RepositoryUrl = repositoryUrl,
        Tech = tech ?? DefaultTech,
        Highlights = highlights ?? Array.Empty<string>()
    };

    private static string ResolveThumbnail(string? webRootPath, string slug, string style)
    {
        if (!string.IsNullOrEmpty(webRootPath))
        {
            foreach (var extension in ScreenshotExtensions)
            {
                var file = Path.Combine(webRootPath, "img", "projects", slug + extension);
                if (File.Exists(file)) return $"/img/projects/{slug}{extension}";
            }
        }

        return $"/img/thumbs/{style}/{slug}.svg";
    }

    public IReadOnlyList<Project> GetAll() => _projects;

    public Project? GetBySlug(string slug) =>
        _projects.FirstOrDefault(p => string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<TermGroup> GetGrouped(string? query = null, string? term = null)
    {
        var filtered = Search(query).ToList();

        return TermOrder
            .Where(t => term is null || string.Equals(t.Term, term, StringComparison.OrdinalIgnoreCase))
            .Select(t => new TermGroup
            {
                Term = t.Term,
                Blurb = t.Blurb,
                Projects = filtered.Where(p => p.Term == t.Term).ToList()
            })
            .Where(g => term is not null || g.Projects.Count > 0)
            .ToList();
    }

    public IReadOnlyList<TermTab> GetTermTabs(string? query = null)
    {
        var filtered = Search(query).ToList();

        return TermOrder
            .Select(t => new TermTab
            {
                Term = t.Term,
                Count = filtered.Count(p => p.Term == t.Term)
            })
            .ToList();
    }

    public string ResolveTerm(string? term)
    {
        return TermOrder.FirstOrDefault(t => string.Equals(t.Term, term, StringComparison.OrdinalIgnoreCase)).Term
            ?? TermOrder[0].Term;
    }

    private IEnumerable<Project> Search(string? query)
    {
        if (string.IsNullOrWhiteSpace(query)) return _projects;

        var value = query.Trim();
        return _projects.Where(p =>
            p.Title.Contains(value, StringComparison.OrdinalIgnoreCase) ||
            p.Summary.Contains(value, StringComparison.OrdinalIgnoreCase) ||
            p.Description.Contains(value, StringComparison.OrdinalIgnoreCase) ||
            p.Role.Contains(value, StringComparison.OrdinalIgnoreCase) ||
            p.Tech.Any(t => t.Contains(value, StringComparison.OrdinalIgnoreCase)));
    }

    public (Project? Previous, Project? Next) GetNeighbours(string slug)
    {
        var ordered = GetGrouped().SelectMany(g => g.Projects).ToList();
        var index = ordered.FindIndex(p => string.Equals(p.Slug, slug, StringComparison.OrdinalIgnoreCase));

        if (index < 0) return (null, null);

        return (
            index > 0 ? ordered[index - 1] : null,
            index < ordered.Count - 1 ? ordered[index + 1] : null);
    }
}
