using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio.Controllers;

public class AccountController : Controller
{
    private readonly IPortfolioAuthenticator _authenticator;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IPortfolioAuthenticator authenticator, ILogger<AccountController> logger)
    {
        _authenticator = authenticator;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction(nameof(ProjectsController.Index), "Projects");
        }

        return View(AddPublishedCredentials(new LoginViewModel { ReturnUrl = returnUrl }));
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(AddPublishedCredentials(model));
        }

        var clientKey = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var result = _authenticator.Validate(model.Username, model.Password, clientKey);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Failed sign-in for {User}", model.Username);
            ModelState.AddModelError(string.Empty, result.Error ?? "Sign-in failed.");
            return View(AddPublishedCredentials(model));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, model.Username.Trim()),
            new Claim("DisplayName", result.DisplayName ?? model.Username.Trim())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(model.RememberMe ? 24 : 2)
            });

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction(nameof(ProjectsController.Index), "Projects");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    private LoginViewModel AddPublishedCredentials(LoginViewModel model)
    {
        var credentials = _authenticator.PublishedCredentials;
        model.PublishedUsername = credentials.Username;
        model.PublishedPassword = credentials.Password;
        return model;
    }
}
