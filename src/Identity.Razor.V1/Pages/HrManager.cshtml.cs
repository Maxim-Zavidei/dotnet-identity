using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V1.Pages;

[Authorize(Policy = "HRManagerOnly")]
public class HrManagerModel : PageModel
{
    public void OnGet()
    {
    }
}
