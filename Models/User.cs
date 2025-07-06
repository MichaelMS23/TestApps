using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }
}
