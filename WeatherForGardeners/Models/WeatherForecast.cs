namespace WeatherForGardeners.Models
{
    public class WeatherForecast
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public string Icon { get; set; }

        public string WeatherClass { get; set; }
    }
}
