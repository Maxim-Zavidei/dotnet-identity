using System.Net.Http.Headers;
using System.Text.Json;
using Identity.Razor.V1.Authorization;
using Identity.Razor.V1.Dto;
using Identity.Razor.V1.Pages.Account.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.Razor.V1.Pages;

[Authorize(Policy = "HRManagerOnly")]
public class HrManagerModel : PageModel
{
    private readonly IHttpClientFactory httpClientFactory;

    [BindProperty]
    public List<WeatherForecastDto> WeatherForecastItems { get; set; } = null!;

    public HrManagerModel(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        // Get token from session
        JwtToken? token = null;
        // First check if we already have a token in the session.
        var strTokenObj = HttpContext.Session.GetString("access_token");
        if (string.IsNullOrWhiteSpace(strTokenObj))
        {
            // If not, contact the authority and get new one.
            token = await Authenticate();
        }
        else
        {
            // If we have a token already in the session perform some additional checks.
            token = JsonSerializer.Deserialize<JwtToken>(strTokenObj)!;
            if (token == null || string.IsNullOrWhiteSpace(token.AccessToken) || token.ExpiresAt <= DateTime.UtcNow)
            {
                token = await Authenticate();
            }
        }
        var httpClient = httpClientFactory.CreateClient("OurWebApi");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        WeatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDto>>("WeatherForecast");
    }

    private async Task<JwtToken> Authenticate()
    {
        var httpClient = httpClientFactory.CreateClient("OurWebApi");
        var response = await httpClient.PostAsJsonAsync("auth", new InputModel
        {
            UserName = "admin",
            Password = "password"
        });
        response.EnsureSuccessStatusCode();
        string strJwt = await response.Content.ReadAsStringAsync();

        // Once we get a token save it in the session
        HttpContext.Session.SetString("access_token", strJwt);

        return JsonSerializer.Deserialize<JwtToken>(strJwt)!;
    }
}
