using System.ComponentModel.DataAnnotations;

namespace Identity.Razor.V1.Pages.Account.Login;

public class InputModel
{
    [Required]
    [Display(Name = "User Name")]
    public required string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}
