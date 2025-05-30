using System.ComponentModel.DataAnnotations.Schema;

namespace Us.Ochlocracy.Data.Entities;

public class UserEntity
{
    public int UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string GivenName { get; init; } = string.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}