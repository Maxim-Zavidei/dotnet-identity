using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Identity.Razor.V2.Models;
using Identity.Razor.V2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V2.Pages;

public class RegisterModel : PageModel
{
    private readonly UserManager<User> userManager;
    private readonly IEmailService emailService;

    [BindProperty]
    public required RegisterViewModel RegisterViewModel { get; set; }

    public RegisterModel(UserManager<User> userManager, IEmailService emailService)
    {
        this.userManager = userManager;
        this.emailService = emailService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // Create the user
        var user = new User
        {
            Email = RegisterViewModel.Email,
            UserName = RegisterViewModel.Email,
            Department = RegisterViewModel.Department,
            Position = RegisterViewModel.Position
        };

        var departmentClaim = new Claim("Department", RegisterViewModel.Department);
        var positionClaim = new Claim("Position", RegisterViewModel.Position);

        var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
        if (result.Succeeded)
        {
            await this.userManager.AddClaimAsync(user, departmentClaim);
            await this.userManager.AddClaimAsync(user, positionClaim);

            var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.PageLink(pageName: "/Account/ConfirmEmail", values: new { userId = user.Id, token = confirmationToken });

            await emailService.SendAsync(
                "test@gmail.com",
                user.Email,
                "Please confirm your email",
                $"Please click on this link to confirm your email address: {confirmationLink}"
            );

            return RedirectToPage("/Account/Login");
        }
        else
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }

            return Page();
        }
    }
}

public class RegisterViewModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }

    [Required]
    [DataType(dataType: DataType.Password)]
    public required string Password { get; set; }

    [Required]
    public required string Department { get; set; }

    [Required]
    public required string Position { get; set; }
}
