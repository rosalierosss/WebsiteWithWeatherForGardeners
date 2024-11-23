using Microsoft.AspNetCore.Mvc;

namespace WeatherForGardeners.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
