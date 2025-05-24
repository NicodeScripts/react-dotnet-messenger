using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}