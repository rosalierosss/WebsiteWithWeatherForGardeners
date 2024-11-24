using Microsoft.AspNetCore.Mvc;
using WeatherForGardeners.Models;

namespace WeatherForGardeners.Controllers
{
    public class TopicsController : Controller
    {
        // Список тем (можно заменить на данные из базы данных)
        private static readonly List<Topic> Topics = new List<Topic>
    {
        new Topic { Id = 1, Title = "Тема 1", Description = "Описание темы 1", Content = "Текст темы 1" },
        new Topic { Id = 2, Title = "Тема 2", Description = "Описание темы 2", Content = "Текст темы 2" },
        new Topic { Id = 3, Title = "Тема 3", Description = "Описание темы 3", Content = "Текст темы 3" },
    };

        public IActionResult Index()
        {
            return View(Topics);
        }

        public IActionResult Details(int id)
        {
            var topic = Topics.FirstOrDefault(t => t.Id == id);
            if (topic == null) return NotFound();
            return PartialView("_TopicDetails", topic);
        }
    }
}
