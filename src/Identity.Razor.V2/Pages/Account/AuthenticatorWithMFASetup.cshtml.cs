using System.ComponentModel.DataAnnotations;
using Identity.Razor.V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;

namespace Identity.Razor.V2.Pages.Account;

[Authorize]
public class AuthenticatorWithMFASetupModel : PageModel
{
    private readonly UserManager<User> userManager;

    [BindProperty]
    public SetupMFAViewModel ViewModel { get; set; }

    [BindProperty]
    public bool Succeded { get; set; }

    public AuthenticatorWithMFASetupModel(UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.ViewModel = new SetupMFAViewModel();
        this.Succeded = false;
    }

    public async Task OnGetAsync()
    {
        var user = await userManager.GetUserAsync(base.User);
        await userManager.ResetAuthenticatorKeyAsync(user!);
        var key = await userManager.GetAuthenticatorKeyAsync(user!);
        this.ViewModel.Key = key!;
        this.ViewModel.QRCodeBytes = GenerateQRCodeBytes("my web app", key!, user!.Email!);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await this.userManager.GetUserAsync(base.User);
        var result = await this.userManager.VerifyTwoFactorTokenAsync(user!, userManager.Options.Tokens.AuthenticatorTokenProvider, this.ViewModel.SecurityCode);

        if (result)
        {
            await userManager.SetTwoFactorEnabledAsync(user!, true);
            this.Succeded = true;
        }
        else
        {
            ModelState.AddModelError("AuthenticatorSetup", "Some went wrong with authenticator setup");
        }
        return Page();
    }

    private Byte[] GenerateQRCodeBytes(string provider, string key, string userEmail)
    {
        var qrCodeGenerator = new QRCodeGenerator();
        var qrCodeData = qrCodeGenerator.CreateQrCode($"otpauth://totp/{provider}:{userEmail}?secret={key}&issuer={provider}", QRCodeGenerator.ECCLevel.Q);
        var qrCode = new BitmapByteQRCode(qrCodeData);
        var qrCodeImageBytes = qrCode.GetGraphic(20);
        return qrCodeImageBytes;
    }
}

public class SetupMFAViewModel
{
    public string Key { get; set; }
    [Required]
    [Display(Name = "Code")]
    public string SecurityCode { get; set; }

    public Byte[] QRCodeBytes { get; set; }
}
