namespace Portfolio.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string Message { get; set; } = "Something went wrong on our side.";
}
