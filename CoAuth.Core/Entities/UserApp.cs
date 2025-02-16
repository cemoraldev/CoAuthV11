using Microsoft.AspNetCore.Identity;

namespace CoAuth.Core.Entities;

public class UserApp : IdentityUser
{
    public string? City { get; set; }
}