using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Globalization;
using WeatherForGardeners.Models;

namespace WeatherForGardeners.Controllers
{
    [Authorize]
    public class WeatherMapController : Controller
    {
        private readonly string _apiKey = "7623034d8696d6abcb4117e9089caf84"; // Ваш API-ключ

        // Отображение карты
        [HttpGet]
        public IActionResult Map()
        {
            return View(); // Это будет представление Map.cshtml
        }

        // Получение погоды
        [HttpGet]
        public async Task<IActionResult> Weather(double lat, double lon, string region)
        {
            string apiUrl = $"https://ru.api.openweathermap.org/data/2.5/forecast?lat={lat.ToString("0.###", CultureInfo.InvariantCulture)}&lon={lon.ToString("0.###", CultureInfo.InvariantCulture)}&appid={_apiKey}&units=metric&lang=ru";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiUrl);
                var weatherData = JObject.Parse(response);

                var forecasts = weatherData["list"]
                    .Select(item => new WeatherForecast
                    {
                        Date = item["dt_txt"].ToString().Split(' ')[0],
                        Time = item["dt_txt"].ToString().Split(' ')[1],
                        Description = item["weather"][0]["description"].ToString(),
                        Temperature = double.Parse(item["main"]["temp"].ToString()),
                        Icon = item["weather"][0]["icon"].ToString(),
                        WeatherClass = GetWeatherClass(item["weather"][0]["description"].ToString())
                    })
                    .GroupBy(f => f.Date)
                    .ToDictionary(g => g.Key, g => g.ToList());

                ViewData["Region"] = region;
                ViewData["Latitude"] = lat;
                ViewData["Longitude"] = lon;

                return View(forecasts); // Это будет представление Weather.cshtml
            }
        }

        private string GetWeatherClass(string description)
        {
            if (description.Contains("облачно") || description.Contains("пасмурно"))
                return "cloudy";
            if (description.Contains("солнечно") || description.Contains("ясно"))
                return "sunny";
            if (description.Contains("дождь"))
                return "rainy";
            if (description.Contains("снег"))
                return "snowy";
            return "default";
        }
    }
}
