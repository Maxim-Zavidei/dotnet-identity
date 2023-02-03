using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V1.Pages;

[Authorize(Policy = "AdminOnly")]
public class SettingsModel : PageModel
{
    public void OnGet()
    {
    }
}

