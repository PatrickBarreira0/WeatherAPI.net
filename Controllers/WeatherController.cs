
using Microsoft.AspNetCore.Mvc;
using WeatherApiGateway.DTOs; 

namespace WeatherApiGateway.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public WeatherController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            try
            {
                var weatherApiKey = _configuration.GetValue<string>("WeatherApiKey"); 

                var client = _httpClientFactory.CreateClient();

                var encodedCity = Uri.EscapeDataString(city);
                var encodedApiKey = Uri.EscapeDataString(weatherApiKey ?? "");
                
                using var response = await client.GetAsync($"https://api.weatherapi.com/v1/current.json?key={encodedApiKey}&q={encodedCity}&aqi=no");

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Erro ao buscar dados do clima na API externa.");
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var fullWeather = System.Text.Json.JsonDocument.Parse(jsonString).RootElement;

                var simplifiedWeather = new SimplifiedWeatherDto
                {
                    City = fullWeather.GetProperty("location").GetProperty("name").GetString(),
                    Region = fullWeather.GetProperty("location").GetProperty("region").GetString(),
                    Country = fullWeather.GetProperty("location").GetProperty("country").GetString(),
                    TempC = fullWeather.GetProperty("current").GetProperty("temp_c").GetDouble(),
                    Condition = fullWeather.GetProperty("current").GetProperty("condition").GetProperty("text").GetString(),
                    IconUrl = "https:" + fullWeather.GetProperty("current").GetProperty("condition").GetProperty("icon").GetString()
                };

                return Ok(simplifiedWeather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro interno no servidor: {ex.Message}");
            }
        }
    }
}