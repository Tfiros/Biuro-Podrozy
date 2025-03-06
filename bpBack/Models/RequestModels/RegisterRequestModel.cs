using System.ComponentModel.DataAnnotations;

public class RegisterRequestModel
{
    [Required]
    [MaxLength(50)]
    public string Login { get; set; }

    [Required]
    [MaxLength(50)]
    public string Password { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(80)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public string Address { get; set; }
}