using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WeatherForGardeners.Pages
{
    public class ResultModel : PageModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public void OnGet(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }
    }
}