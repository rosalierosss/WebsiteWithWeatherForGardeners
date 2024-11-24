using Microsoft.AspNetCore.Mvc;
using WeatherForGardeners.DTOs;
using WeatherForGardeners.Models;
using WeatherForGardeners.ViewModels;

namespace WeatherForGardeners.Controllers
{
    [Route("DayDetails")]
    public class DayDetailsController : Controller
    {
        private static readonly Dictionary<DateTime, List<TaskItem>> TasksData = new()
        {
            { DateTime.Today, new List<TaskItem>
                {
                    new TaskItem { Id = 1, Title = "Задача 1", Description = "Описание задачи 1", IsCompleted = false },
                    new TaskItem { Id = 2, Title = "Задача 2", Description = "Описание задачи 2", IsCompleted = true }
                }
            },
            { DateTime.Today.AddDays(1), new List<TaskItem>
                {
                    new TaskItem { Title = "Задача 3", Description = "Описание задачи 3", IsCompleted = false }
                }
            }
        };

        [HttpGet("Index")]
        public IActionResult Index(string date)
        {
            if (DateTime.TryParse(date, out var parsedDate))
            {
                TasksData.TryGetValue(parsedDate, out var tasks);

                var viewModel = new DayDetailsViewModel
                {
                    Date = parsedDate,
                    Tasks = tasks ?? new List<TaskItem>()
                };

                return View(viewModel);
            }

            return BadRequest("Неверный формат даты");
        }

        [HttpPost("UpdateTaskStatus")]
        public IActionResult UpdateTaskStatus([FromBody] List<TaskStatusUpdateModel> updates)
        {
            if (updates == null || !updates.Any())
            {
                return BadRequest(new { success = false, message = "Нет данных для обновления." });
            }

            foreach (var update in updates)
            {
                foreach (var tasks in TasksData.Values)
                {
                    var task = tasks.FirstOrDefault(t => t.Id == update.TaskId);
                    if (task != null)
                    {
                        task.IsCompleted = update.IsCompleted;
                    }
                }
            }

            return Ok(new { success = true });
        }
    }
}
