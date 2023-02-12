using Identity.Razor.V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V2.Pages.Account;

[Authorize]
public class AuthenticatorWithMFASetupModel : PageModel
{
    private readonly UserManager<User> userManager;

    [BindProperty]
    public SetupMFAViewModel ViewModel { get; set; }

    public AuthenticatorWithMFASetupModel(UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.ViewModel = new SetupMFAViewModel();
    }

    public async Task OnGetAsync()
    {
        var user = await userManager.GetUserAsync(base.User);
        await userManager.ResetAuthenticatorKeyAsync(user!);
        var key = await userManager.GetAuthenticatorKeyAsync(user!);
        this.ViewModel.Key = key!;
    }
}

public class SetupMFAViewModel
{
    public string Key { get; set; }
}
