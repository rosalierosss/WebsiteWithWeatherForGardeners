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

        [HttpPost("SaveAllChanges")]
        public IActionResult SaveAllChanges([FromQuery] DateTime date, [FromBody] AllChangesModel changes)
        {
            if (changes == null)
            {
                return BadRequest(new { success = false, message = "Нет данных для сохранения." });
            }

            // Обработка добавленных задач
            foreach (var newTask in changes.Added)
            {
                var task = new TaskItem
                {
                    Id = TasksData.Values.SelectMany(t => t).Max(t => t.Id) + 1,
                    Title = newTask.Title,
                    Description = newTask.Description,
                    IsCompleted = false
                };

                if (!TasksData.ContainsKey(date))
                {
                    TasksData[date] = new List<TaskItem>();
                }

                TasksData[date].Add(task);
            }

            // Обработка отредактированных задач
            foreach (var editedTask in changes.Edited)
            {
                foreach (var tasks in TasksData.Values)
                {
                    var task = tasks.FirstOrDefault(t => t.Id == editedTask.TaskId);
                    if (task != null)
                    {
                        task.Title = editedTask.Title;
                        task.Description = editedTask.Description;
                    }
                }
            }

            // Обработка обновления статусов
            foreach (var statusUpdate in changes.UpdatedStatuses)
            {
                foreach (var tasks in TasksData.Values)
                {
                    var task = tasks.FirstOrDefault(t => t.Id == statusUpdate.TaskId);
                    if (task != null)
                    {
                        task.IsCompleted = statusUpdate.IsCompleted;
                    }
                }
            }

            return Ok(new { success = true });
        }


    }
}
