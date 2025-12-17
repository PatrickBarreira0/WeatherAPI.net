
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace WeatherApiGateway.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-API-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key não foi informada no cabeçalho X-API-Key.");
                return;
            }

            var appApiKey = configuration.GetValue<string>("ApiKey");

            if (string.IsNullOrEmpty(appApiKey) || !appApiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401; // ou 403 Forbidden
                await context.Response.WriteAsync("API Key inválida.");
                return;
            }

            await _next(context);
        }
    }
}