

using Microsoft.AspNetCore.Mvc;

namespace ChefHesab.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetFoodStuffVm()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7111/api/FoodStuffApi/GetFoodStuff");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsStringAsync();
                return Ok(product);
            }
            return Ok();
        }
    }
}