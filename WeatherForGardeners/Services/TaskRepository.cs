using WeatherForGardeners.Data;
using WeatherForGardeners.Models;

namespace WeatherForGardeners.Services
{
    public class TaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        // Получить задачи на указанный день
        public List<TaskItem> GetTasksByDate(DateTime date)
        {
            return _context.TaskItems
                .Where(task => task.DateTime.Date == date.Date)
                .ToList();
        }

        // Добавить новую задачу
        public TaskItem AddTask(DateTime date, string title, string description)
        {
            var newTask = new TaskItem
            {
                Title = title,
                Description = description,
                IsCompleted = false,
                DateTime = date
            };

            _context.TaskItems.Add(newTask);
            _context.SaveChanges();

            return newTask;
        }

        // Обновить задачу
        public bool UpdateTask(int taskId, Action<TaskItem> updateAction)
        {
            var task = _context.TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                updateAction(task);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
