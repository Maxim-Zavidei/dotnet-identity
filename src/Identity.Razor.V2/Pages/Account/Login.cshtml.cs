using Identity.Razor.V2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V2.Pages.Account.Login;

public class LoginModel : PageModel
{
    private readonly SignInManager<User> signInManager;

    public LoginModel(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }

    // Provides 2 way data binding
    [BindProperty]
    public required InputModel InputModel { get; set; }

    [BindProperty]
    public IEnumerable<AuthenticationScheme> ExternalLoginProviders { get; set; }

    public async Task OnGet()
    {
        this.ExternalLoginProviders = await signInManager.GetExternalAuthenticationSchemesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var result = await signInManager.PasswordSignInAsync(this.InputModel.Email, this.InputModel.Password, this.InputModel.RememberMe, false);

        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        else
        {
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("/Account/LoginTwoFactorWithAuthenticator", new
                {
                    RememberMe = this.InputModel.RememberMe
                });
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Login", "You are locked out.");
            }
            else
            {
                ModelState.AddModelError("Login", "Failed to log in.");
            }

            return Page();
        }
    }

    public IActionResult OnPostLoginExternally(string provider)
    {
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, null);
        properties.RedirectUri = Url.Action("", "");
        return Challenge(properties, provider);
    }
}
