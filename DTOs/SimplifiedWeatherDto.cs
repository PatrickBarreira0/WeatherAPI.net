namespace WeatherApiGateway.DTOs
{
    public class SimplifiedWeatherDto
    {
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public double TempC { get; set; }
        public string? Condition { get; set; }
        public string? IconUrl { get; set; }
    }
}