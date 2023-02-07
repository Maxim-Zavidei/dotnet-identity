namespace Identity.Razor.V2.Services;

public interface IEmailService
{
    Task SendAsync(string from, string to, string subject, string body);
}
