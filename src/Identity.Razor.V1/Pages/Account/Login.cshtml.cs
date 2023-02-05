using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V1.Pages.Account.Login;

public class LoginModel : PageModel
{
    // Provides 2 way data binding
    [BindProperty]
    public required InputModel InputModel { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // Verify the credentials
        if (InputModel.UserName == "admin" && InputModel.Password == "password")
        {
            // Create the security context
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@gmail.com"),
                new Claim("Department", "HR"),
                new Claim("Admin", "true"),
                new Claim("Manager", "true"),
                new Claim("EmploymentDate", "2021-05-01")
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            // Here we can initialize some properties for the authentication
            // e.g if the authentication cookie should be persistent or not.
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = InputModel.RememberMe
            };

            // Serializes the claims principal into a string, encrypts that string and saves it as a cookie in the HttpContext
            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

            return RedirectToPage("/Index");
        }
        return Page();
    }
}
