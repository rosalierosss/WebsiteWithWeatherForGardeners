using Microsoft.AspNetCore.Mvc;
using WeatherForGardeners.Models;

namespace WeatherForGardeners.Controllers
{
    public class HomeController : Controller
    {
        private List<Plant> plants;

        public HomeController()
        {
            plants = new List<Plant>()
            {
                new Plant { Id = 1, Name = "Какое то растение", Description = "Это растение очень красивое. Проверка длинного описания. Проверка длинного описания. Проверка длинного описания. Проверка длинного описания. Проверка длинного описания. Проверка длинного описания. Проверка длинного описания.", PhotoUrl = "/images/clover.jpg" },
                new Plant { Id = 2, Name = "Plant 2", Description = "Description 2", PhotoUrl = "/images/hibiscus.jpeg" },
                new Plant { Id = 3, Name = "Plant 3", Description = "Description 3", PhotoUrl = "images/cat.jpeg" },
                new Plant { Id = 4, Name = "Plant 4", Description = "Description 4", PhotoUrl = "/images/hibiscus.jpeg" },
            };

            for (int i = 5; i < 16; i++)
            {
                plants.Add(new Plant
                {
                    Id = i,
                    Name = $"Plant {i}",
                    Description = $"Description {i}",
                    PhotoUrl = $"/images/plant{i}.jpg"
                });
            }
        }

        public IActionResult Index()
        {
            return View(plants);
        }
    }
}
