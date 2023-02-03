using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V1.Pages;

[Authorize(Policy = "MustBeFromHrDepartment")]
public class HumanResourceModel : PageModel
{
    public void OnGet()
    {

    }
}
