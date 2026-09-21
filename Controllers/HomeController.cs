using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio.Controllers;

public class HomeController : Controller
{
    private readonly IPortfolioAuthenticator _authenticator;

    public HomeController(IPortfolioAuthenticator authenticator)
    {
        _authenticator = authenticator;
    }

    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction(nameof(ProjectsController.Index), "Projects");
        }

        var (username, password) = _authenticator.PublishedCredentials;

        return View(new LandingViewModel
        {
            Username = username,
            Password = password
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
