using System.Security.Claims;
using Identity.Razor.V2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Razor.V2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<User> signInManager;

    public AccountController(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }

    public async Task<IActionResult> ExternalLoginCallback()
    {
        var loginInfo = await signInManager.GetExternalLoginInfoAsync();
        var emailClaim = loginInfo!.Principal.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email);
        var userClaim = loginInfo!.Principal.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Name);

        if (emailClaim != null && userClaim != null)
        {
            var user = new User
            {
                Email = emailClaim.Value,
                UserName = userClaim.Value,
                Department = "Some department",
                Position = "Some position"
            };
            await signInManager.SignInAsync(user, false);
        }
        return RedirectToPage("/Index");
    }
}
