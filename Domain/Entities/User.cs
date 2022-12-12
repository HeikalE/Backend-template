using Domain.Enums;

namespace Domain.Entities;

public class User : Base
{
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public UserRole Role { get; set; }
}
