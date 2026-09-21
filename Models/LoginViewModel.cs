using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Enter your username.")]
    [StringLength(50)]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter your password.")]
    [StringLength(100)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Keep me signed in")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
    public string PublishedUsername { get; set; } = string.Empty;
    public string PublishedPassword { get; set; } = string.Empty;
}
