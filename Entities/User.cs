using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Entities;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = "email@default.com";
    public string Password { get; set; } = "DefaultPassword123!";
    [Key] public string Username { get; set; } = string.Empty;
}