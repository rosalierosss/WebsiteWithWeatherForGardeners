using WeatherForGardeners.Models;

namespace WeatherForGardeners.ViewModels
{
    public class DayDetailsViewModel
    {
        public DateTime Date { get; set; }
        public List<TaskItem> Tasks { get; set; }
        public string Content { get; set; }
    }

}
