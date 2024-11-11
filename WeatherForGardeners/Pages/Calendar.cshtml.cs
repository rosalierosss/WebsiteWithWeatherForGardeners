using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherForGardeners.Models;
using WeatherForGardeners.ViewModels;

namespace WeatherForGardeners.Pages
{
    public class CalendarModel : PageModel
    {
        public CalendarViewModel CalendarData { get; set; }

        public async Task<IActionResult> OnGetAsync(int? month, int? year)
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
            CalendarData = new CalendarViewModel
            {
                Year = selectedYear,
                Month = selectedMonth,
                Days = await GetDaysWithTasks(selectedYear, selectedMonth)
            };

            return Page();
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
            return await Task.FromResult(new Random().Next(0, 5));
        }
    }
}
