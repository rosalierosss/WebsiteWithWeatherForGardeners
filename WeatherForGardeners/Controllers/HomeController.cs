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
                new Plant { Id = 1, Name = "Plant 1", Description = "Description 1", PhotoUrl = "/images/clover.jpg" },
                new Plant { Id = 2, Name = "Plant 2", Description = "Description 2", PhotoUrl = "/images/hibiscus.jpeg" },
            };

            for (int i = 3; i < 16; i++)
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
