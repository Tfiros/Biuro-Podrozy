using System.ComponentModel.DataAnnotations;

namespace TinProjektBackend.Models.RequestModels;

public class LoginRequestModel
{
    [Required]
    public string Login { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}