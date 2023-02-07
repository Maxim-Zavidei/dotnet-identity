using System.Net;
using System.Net.Mail;
using Identity.Razor.V2.Options;
using Microsoft.Extensions.Options;

namespace Identity.Razor.V2.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<SmtpOptions> smtpOptions;

    public EmailService(IOptions<SmtpOptions> smtpOptions)
    {
        this.smtpOptions = smtpOptions;
    }

    public async Task SendAsync(string from, string to, string subject, string body)
    {
        var message = new MailMessage(from, to, subject, body);

        using (var emailClient = new SmtpClient(smtpOptions.Value.Host, smtpOptions.Value.Port))
        {
            emailClient.Credentials = new NetworkCredential(smtpOptions.Value.User, smtpOptions.Value.Password);
            // await emailClient.SendMailAsync(message);
        }
    }
}
