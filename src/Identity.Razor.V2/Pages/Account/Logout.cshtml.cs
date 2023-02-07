using Identity.Razor.V2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V2.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<User> signInManager;

    public LogoutModel(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await signInManager.SignOutAsync();
        return RedirectToPage("/Account/Login");
    }
}
