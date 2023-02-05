using Identity.Razor.V1.Dto;
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
        var httpClient = httpClientFactory.CreateClient("OurWebApi");
        WeatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDto>>("WeatherForecast");
    }
}
