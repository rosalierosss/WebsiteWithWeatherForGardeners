using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForGardeners.Models;
using WeatherForGardeners.Services;
using WeatherForGardeners.ViewModels;

namespace WeatherForGardeners.Controllers
{
    [Authorize]
    [Route("Calendar")]
    public class CalendarController : Controller
    {
        private readonly TaskRepository _taskRepository;

        public Dictionary<int, List<string>> MonthlyRecommendations { get; set; } = new Dictionary<int, List<string>>
        {
            { 10, new List<string> // Октябрь
                {
                    "<b>Подготовка сада к зиме:</b> Проверьте все кустарники и деревья, обрежьте сухие ветки. <img src='/images/autumn_garden.webp' alt='Осенний сад' style='width:100%; max-width:300px; margin-top:10px;'>",
                    "<b>Посадка луковичных цветов:</b> Это время для посадки тюльпанов и нарциссов, чтобы они зацвели весной. <img src='/images/bulbs_planting.webp' alt='Посадка луковиц' style='width:100%; max-width:300px; margin-top:10px;'>"
                }
            },
            { 11, new List<string> // Ноябрь
                {
                    "<b>Укрытие растений от морозов:</b> Используйте агроволокно или мульчу для защиты нежных культур. <img src='/images/plant_cover.webp' alt='Укрытие растений' style='width:100%; max-width:300px; margin-top:10px;'>",
                    "<b>Соберите последние урожаи:</b> Проверьте теплицы на наличие оставшихся овощей и зелени."
                }
            },
            { 12, new List<string> // Декабрь
                {
                     "<b>Как правильно ухаживать за розами:</b> Убедитесь, что розы получают достаточно солнечного света. Поливайте их утром, чтобы избежать болезней. <img src='/images/rose.webp' alt='Уход за розами' style='width:100%; max-width:300px; margin-top:10px;'>",

                    "<b>Время для посадки овощей:</b> В весенние месяцы начните с посадки редиса, моркови и зелени. Используйте органическое удобрение для лучшего роста. <img src='/images/ovochi.webp' alt='Посадка овощей' style='width:100%; max-width:300px; margin-top:10px;'>",

                    "<b>Зимний уход за садом:</b> Обеспечьте укрытие для растений, которые не переносят морозы. Соберите опавшую листву и используйте ее как мульчу."
                }
            }
        };

        // Получение рекомендаций для текущего месяца
        public List<string> GetRecommendationsForMonth(int month)
        {
            return MonthlyRecommendations.TryGetValue(month, out var recommendations) ? recommendations : new List<string>();
        }


        public CalendarController(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index(int? month, int? year)
        {
            var selectedMonth = month ?? DateTime.Now.Month;
            var selectedYear = year ?? DateTime.Now.Year;

            // Обработка выхода за пределы месяца
            if (selectedMonth > 12)
            {
                selectedMonth = 1;
                selectedYear++;
            }
            else if (selectedMonth < 1)
            {
                selectedMonth = 12;
                selectedYear--;
            }

            // Получение данных для календаря
            var calendarData = new CalendarViewModel
            {
                Year = selectedYear,
                Month = selectedMonth,
                Days = await GetDaysWithTasks(selectedYear, selectedMonth),
                Recommendations = GetRecommendationsForMonth(selectedMonth),
            };

            return View(calendarData); // Передача данных в представление
        }

        private async Task<List<CalendarDay>> GetDaysWithTasks(int year, int month)
        {
            var days = new List<CalendarDay>();
            var daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                int taskCount = await GetTaskCountForDay(date);

                days.Add(new CalendarDay
                {
                    Date = date,
                    TaskCount = taskCount,
                    IsToday = date.Date == DateTime.Today
                });
            }

            return days;
        }

        private async Task<int> GetTaskCountForDay(DateTime date)
        {
            // Получение количества задач на конкретный день через репозиторий
            var tasks = _taskRepository.GetTasksByDate(date);
            return tasks.Count;
        }
    }
}
