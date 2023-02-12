using System.ComponentModel.DataAnnotations;
using Identity.Razor.V2.Models;
using Identity.Razor.V2.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V2.Pages.Account;

public class LoginTwoFactorModel : PageModel
{
    private readonly UserManager<User> userManager;
    private readonly IEmailService emailService;
    private readonly SignInManager<User> signInManager;

    [BindProperty]
    public EmailMFA EmailMFA { get; set; }

    public LoginTwoFactorModel(UserManager<User> userManager, IEmailService emailService, SignInManager<User> signInManager)
    {
        this.userManager = userManager;
        this.emailService = emailService;
        this.signInManager = signInManager;
        this.EmailMFA = new();
    }

    public async Task OnGetAsync(string email, bool rememberMe)
    {
        var user = await userManager.FindByEmailAsync(email);

        this.EmailMFA.SecurityCode = string.Empty;
        this.EmailMFA.RememberMe = rememberMe;

        // Generate the security code
        var securityCode = await userManager.GenerateTwoFactorTokenAsync(user!, "Email");

        // Send to the user
        await emailService.SendAsync("from", "to", "OTP code", $"Use  this code as the otp: {securityCode}");

    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var result = await signInManager.TwoFactorSignInAsync("Email", this.EmailMFA.SecurityCode, this.EmailMFA.RememberMe, false);

        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        else
        {
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Login2FA", "You are locked out.");
            }
            else
            {
                ModelState.AddModelError("Login2FA", "Failed to log in.");
            }

            return Page();
        }
    }
}

public class EmailMFA
{
    [Required]
    [Display(Name = "Security Code")]
    public string SecurityCode { get; set; }
    [Required]
    public bool RememberMe { get; set; }
}
