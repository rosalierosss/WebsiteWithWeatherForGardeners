namespace WeatherForGardeners.Models
{
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public int TaskCount { get; set; }
        public bool IsToday { get; set; }
    }
}
