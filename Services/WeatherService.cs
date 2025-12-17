using System.Text.Json;
using WeatherApiGateway.DTOs;

namespace WeatherApiGateway.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<SimplifiedWeatherDto?> GetWeatherAsync(string city)
        {
            var weatherApiKey = _configuration.GetValue<string>("WeatherApiKey");
            if (string.IsNullOrWhiteSpace(weatherApiKey) || string.IsNullOrWhiteSpace(city))
            {
                return null;
            }

            var client = _httpClientFactory.CreateClient();

            var encodedCity = Uri.EscapeDataString(city);
            var encodedApiKey = Uri.EscapeDataString(weatherApiKey);

            using var response = await client.GetAsync($"https://api.weatherapi.com/v1/current.json?key={encodedApiKey}&q={encodedCity}&aqi=no");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(jsonString);
            var fullWeather = document.RootElement;

            return new SimplifiedWeatherDto
            {
                City = fullWeather.GetProperty("location").GetProperty("name").GetString(),
                Region = fullWeather.GetProperty("location").GetProperty("region").GetString(),
                Country = fullWeather.GetProperty("location").GetProperty("country").GetString(),
                TempC = fullWeather.GetProperty("current").GetProperty("temp_c").GetDouble(),
                Condition = fullWeather.GetProperty("current").GetProperty("condition").GetProperty("text").GetString(),
                IconUrl = "https:" + fullWeather.GetProperty("current").GetProperty("condition").GetProperty("icon").GetString()
            };
        }
    }
}

