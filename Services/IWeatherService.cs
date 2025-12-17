using WeatherApiGateway.DTOs;

namespace WeatherApiGateway.Services
{
    public interface IWeatherService
    {
        Task<SimplifiedWeatherDto?> GetWeatherAsync(string city);
    }
}

