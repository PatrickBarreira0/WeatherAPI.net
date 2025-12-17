
using Microsoft.AspNetCore.Mvc;
using WeatherApiGateway.Services;

namespace WeatherApiGateway.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            try
            {
                var simplifiedWeather = await _weatherService.GetWeatherAsync(city);
                if (simplifiedWeather is null)
                {
                    return NotFound();
                }

                return Ok(simplifiedWeather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro interno no servidor: {ex.Message}");
            }
        }
    }
}