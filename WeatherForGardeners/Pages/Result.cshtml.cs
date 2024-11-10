using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WeatherForGardeners.Pages
{
    public class WeatherForecast
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public string Icon { get; set; }
    }

    public class ResultModel : PageModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Region { get; set; }
        public Dictionary<string, List<WeatherForecast>> DailyForecasts { get; set; } = new();

        private readonly string apiKey = "7623034d8696d6abcb4117e9089caf84"; // Ваш API-ключ

        public async Task OnGet(double lat, double lon, string region)
        {
            Latitude = lat;
            Longitude = lon;
            Region = region;

            string apiUrl = $"https://ru.api.openweathermap.org/data/2.5/forecast?lat={lat.ToString("0.###", CultureInfo.InvariantCulture)}&lon={lon.ToString("0.###", CultureInfo.InvariantCulture)}&appid={apiKey}&units=metric&lang=ru";

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
                        Icon = item["weather"][0]["icon"].ToString()
                    })
                    .GroupBy(f => f.Date)
                    .ToDictionary(g => g.Key, g => g.ToList());

                DailyForecasts = forecasts;
            }
        }

        public string GetWeatherClass(string description)
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

        public string GetIconClass(string icon)
        {
            return icon switch
            {
                "01d" => "wi-day-sunny",
                "01n" => "wi-night-clear",
                "02d" => "wi-day-cloudy",
                "02n" => "wi-night-alt-cloudy",
                "03d" => "wi-cloud",
                "03n" => "wi-cloud",
                "04d" => "wi-cloudy",
                "04n" => "wi-cloudy",
                "09d" => "wi-showers",
                "09n" => "wi-showers",
                "10d" => "wi-day-rain",
                "10n" => "wi-night-alt-rain",
                "11d" => "wi-thunderstorm",
                "11n" => "wi-thunderstorm",
                "13d" => "wi-snow",
                "13n" => "wi-snow",
                "50d" => "wi-fog",
                "50n" => "wi-fog",
                _ => "wi-na"
            };
        }
    }
}
