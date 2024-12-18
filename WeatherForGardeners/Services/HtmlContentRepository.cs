namespace WeatherForGardeners.Services
{
    public class HtmlContentRepository
    {
        // Хранилище HTML-контента, привязанного к дате
        private readonly Dictionary<DateTime, string> _htmlData = new()
    {
        { DateTime.Today, "<b>Как правильно ухаживать за розами:</b> Убедитесь, что розы получают достаточно солнечного света. Поливайте их утром, чтобы избежать болезней. <img src='/images/rose.webp' alt='Уход за розами' style='width:100%; max-width:300px; margin-top:10px;'>" },
        { DateTime.Today.AddDays(1), "<b>Как правильно ухаживать за розами:</b> Убедитесь, что розы получают достаточно солнечного света. Поливайте их утром, чтобы избежать болезней. <img src='/images/rose.webp' alt='Уход за розами' style='width:100%; max-width:300px; margin-top:10px;'>" }
    };

        // Получить HTML-контент для указанной даты
        public string GetHtmlByDate(DateTime date)
        {
            return _htmlData.TryGetValue(date, out var htmlContent)
                ? htmlContent
                : $"<p>На {date:dd.MM.yyyy} данных нет.</p>";
        }

        // Добавить или обновить HTML-контент для указанной даты
        public void AddOrUpdateHtml(DateTime date, string htmlContent)
        {
            _htmlData[date] = htmlContent;
        }

        // Удалить HTML-контент для указанной даты
        public bool RemoveHtmlByDate(DateTime date)
        {
            return _htmlData.Remove(date);
        }

        // Получить все даты, для которых есть HTML-контент
        public List<DateTime> GetAllDates()
        {
            return _htmlData.Keys.ToList();
        }
    }

}
