namespace Services.Abstraction.Models;

public class AuthModel
{
    public required string Message { get; set; }
    public string? Token { get; set; }
    public bool IsAuthenticated { get; set; }
    public DateTime ExpiresOn { get; set; }
}
