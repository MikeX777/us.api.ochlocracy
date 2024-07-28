namespace Us.Ochlocracy.Model.Users;

public class User
{
    public int UserId { get; set; }
    public string GivenName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
}