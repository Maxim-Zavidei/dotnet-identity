using System.ComponentModel.DataAnnotations;
using Identity.Razor.V2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V2.Pages.Account;

public class LoginTwoFactorWithAuthenticatorModel : PageModel
{
    private readonly SignInManager<User> signInManager;

    [BindProperty]
    public AuthenticatorMFA AuthenticatorMFA { get; set; }

    public LoginTwoFactorWithAuthenticatorModel(SignInManager<User> signInManager)
    {
        this.AuthenticatorMFA = new AuthenticatorMFA();
        this.signInManager = signInManager;
    }

    public void OnGet(bool rememberMe)
    {
        this.AuthenticatorMFA.SecurityCode = string.Empty;
        this.AuthenticatorMFA.RememberMe = rememberMe;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(this.AuthenticatorMFA.SecurityCode, this.AuthenticatorMFA.RememberMe, false);

        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        else
        {
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Authenticator2FA", "You are locked out.");
            }
            else
            {
                ModelState.AddModelError("Authenticator2FA", "Failed to log in.");
            }
            return Page();
        }
    }
}

public class AuthenticatorMFA
{
    [Required]
    [Display(Name = "Code")]
    public string SecurityCode { get; set; }

    public bool RememberMe { get; set; }
}
