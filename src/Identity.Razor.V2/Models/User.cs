using Microsoft.AspNetCore.Identity;

namespace Identity.Razor.V2.Models;

public class User : IdentityUser
{
    public required string Department { get; set; }
    public required string Position { get; set; }
}
