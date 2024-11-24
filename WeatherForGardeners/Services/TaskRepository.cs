using WeatherForGardeners.Models;

namespace WeatherForGardeners.Services
{
    public class TaskRepository
    {
        private readonly Dictionary<DateTime, List<TaskItem>> _tasksData = new()
    {
        { DateTime.Today, new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Задача 1", Description = "Описание задачи 1", IsCompleted = false },
                new TaskItem { Id = 2, Title = "Задача 2", Description = "Описание задачи 2", IsCompleted = true }
            }
        },
        { DateTime.Today.AddDays(1), new List<TaskItem>
            {
                new TaskItem { Id = 3, Title = "Задача 3", Description = "Описание задачи 3", IsCompleted = false }
            }
        }
    };

        // Получить задачи на указанный день
        public List<TaskItem> GetTasksByDate(DateTime date)
        {
            return _tasksData.TryGetValue(date, out var tasks) ? tasks : new List<TaskItem>();
        }

        // Добавить новую задачу
        public TaskItem AddTask(DateTime date, string title, string description)
        {
            var newTask = new TaskItem
            {
                Id = _tasksData.Values.SelectMany(t => t).DefaultIfEmpty().Max(t => t?.Id ?? 0) + 1,
                Title = title,
                Description = description,
                IsCompleted = false
            };

            if (!_tasksData.ContainsKey(date))
            {
                _tasksData[date] = new List<TaskItem>();
            }

            _tasksData[date].Add(newTask);
            return newTask;
        }

        // Обновить задачу
        public bool UpdateTask(int taskId, Action<TaskItem> updateAction)
        {
            foreach (var tasks in _tasksData.Values)
            {
                var task = tasks.FirstOrDefault(t => t.Id == taskId);
                if (task != null)
                {
                    updateAction(task);
                    return true;
                }
            }
            return false;
        }
    }

}
