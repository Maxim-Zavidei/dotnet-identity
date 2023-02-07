using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V2.Pages;


public class RegisterModel : PageModel
{
    private readonly UserManager<IdentityUser> userManager;

    [BindProperty]
    public required RegisterViewModel RegisterViewModel { get; set; }

    public RegisterModel(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // Validating email address


        // Create the user
        var user = new IdentityUser
        {
            Email = RegisterViewModel.Email,
            UserName = RegisterViewModel.Email
        };

        var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
        if (result.Succeeded)
        {
            var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            return Redirect(Url.PageLink(pageName: "/Account/ConfirmEmail", values: new { userId = user.Id, token = confirmationToken })!);

            //return RedirectToPage("/Account/Login");
        }
        else
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }

            return Page();
        }
    }
}

public class RegisterViewModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }

    [Required]
    [DataType(dataType: DataType.Password)]
    public required string Password { get; set; }
}
