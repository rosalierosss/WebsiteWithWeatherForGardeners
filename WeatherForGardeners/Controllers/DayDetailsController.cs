using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForGardeners.DTOs;
using WeatherForGardeners.Models;
using WeatherForGardeners.Services;
using WeatherForGardeners.ViewModels;

namespace WeatherForGardeners.Controllers
{
    [Authorize]
    [Route("DayDetails")]
    public class DayDetailsController : Controller
    {
        private readonly TaskRepository _taskRepository;
        private readonly HtmlContentRepository _htmlContentRepository;

        public DayDetailsController(TaskRepository taskRepository, HtmlContentRepository htmlContentRepository)
        {
            _taskRepository = taskRepository;
            _htmlContentRepository = htmlContentRepository;
        }


        [HttpGet("Index")]
        public IActionResult Index(string date)
        {
            if (DateTime.TryParse(date, out var parsedDate))
            {
                var tasks = _taskRepository.GetTasksByDate(parsedDate);
                var html = _htmlContentRepository.GetHtmlByDate(parsedDate);

                var viewModel = new DayDetailsViewModel
                {
                    Date = parsedDate,
                    Tasks = tasks,
                    HtmlContent = html
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
                _taskRepository.AddTask(date, newTask.Title, newTask.Description);
            }

            // Обработка отредактированных задач
            foreach (var editedTask in changes.Edited)
            {
                _taskRepository.UpdateTask(editedTask.TaskId, task =>
                {
                    task.Title = editedTask.Title;
                    task.Description = editedTask.Description;
                });
            }

            // Обработка обновления статусов
            foreach (var statusUpdate in changes.UpdatedStatuses)
            {
                _taskRepository.UpdateTask(statusUpdate.TaskId, task =>
                {
                    task.IsCompleted = statusUpdate.IsCompleted;
                });
            }

            return Ok(new { success = true });
        }


    }
}
