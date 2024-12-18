using WeatherForGardeners.Models;

namespace WeatherForGardeners.ViewModels
{
    public class CalendarViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<CalendarDay> Days { get; set; }
        public List<string> Recommendations { get; set; }
    }
}
