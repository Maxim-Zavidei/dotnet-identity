using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V2.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<IdentityUser> signInManager;

    public LogoutModel(SignInManager<IdentityUser> signInManager)
    {
        this.signInManager = signInManager;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await signInManager.SignOutAsync();
        return RedirectToPage("/Account/Login");
    }
}
