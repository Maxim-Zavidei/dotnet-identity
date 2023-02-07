namespace Identity.Razor.V2.Options;

public class SmtpOptions
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string User { get; set; }
    public required string Password { get; set; }
}
