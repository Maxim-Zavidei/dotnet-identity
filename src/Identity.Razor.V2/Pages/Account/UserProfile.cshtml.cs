using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Identity.Razor.V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V2.Pages.Account;

[Authorize]
public class UserProfileModel : PageModel
{
    private readonly UserManager<User> userManager;

    [BindProperty]
    public UserProfileViewModel UserProfile { get; set; }

    [BindProperty]
    public string? SuccessMessage { get; set; }

    public UserProfileModel(UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.UserProfile = new UserProfileViewModel();
        this.SuccessMessage = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        this.SuccessMessage = string.Empty;
        var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();

        this.UserProfile.Email = User.Identity!.Name;
        this.UserProfile.Department = departmentClaim?.Value;
        this.UserProfile.Position = positionClaim?.Value;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        this.SuccessMessage = string.Empty;
        if (!ModelState.IsValid) return Page();

        try
        {
            var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();

            // When replacing claims, the old type has to be the same as the new one.
            await userManager.ReplaceClaimAsync(user, departmentClaim, new Claim(departmentClaim.Type.ToString(), UserProfile.Department!));
            await userManager.ReplaceClaimAsync(user, positionClaim, new Claim(positionClaim.Type.ToString(), UserProfile.Position!));
        }
        catch
        {
            ModelState.AddModelError("UserProfile", "Error occured when saving user profile.");
            this.SuccessMessage = "The user profile was not saved successfully";
        }

        this.SuccessMessage = "The user profile is saved successfully";

        return Page();
    }

    private async Task<(User, Claim, Claim)> GetUserInfoAsync()
    {
        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        var claims = await userManager.GetClaimsAsync(user!);
        var departmentClaim = claims.FirstOrDefault(e => e.Type == "Department");
        var positionClaim = claims.FirstOrDefault(e => e.Type == "Position");

        return (user!, departmentClaim!, positionClaim!);
    }
}

public class UserProfileViewModel
{

    public string? Email { get; set; }
    
    [Required]
    public string? Department { get; set; }

    [Required]
    public string? Position { get; set; }
}
