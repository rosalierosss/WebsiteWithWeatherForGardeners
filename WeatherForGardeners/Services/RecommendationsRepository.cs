using WeatherForGardeners.Data;

namespace WeatherForGardeners.Services
{
    public class RecommendationsRepository
    {
        private AppDbContext _appDbContext;
        public RecommendationsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        // Получить HTML-контент для указанной даты
        public string GetHtmlByDate(DateTime date)
        {
            var dayRec = _appDbContext.DayRecommendations.FirstOrDefault(r => r.DateTime.Date == date.Date);
            if (dayRec == null)
            {
                return $"<p>На {date:dd.MM.yyyy} данных нет.</p>";
            } else
            {
                return dayRec.Content;
            }
        }

        // Добавить или обновить HTML-контент для указанной даты
        public void AddOrUpdateHtml(DateTime date, string htmlContent)
        {
            var dayRec = _appDbContext.DayRecommendations.FirstOrDefault(r => r.DateTime.Date == date.Date);

            if (dayRec == null)
            {
                // Если запись не найдена, добавляем новую
                _appDbContext.DayRecommendations.Add(new Models.DayRecommendation
                {
                    DateTime = date,
                    Content = htmlContent
                });
            }
            else
            {
                // Если запись найдена, обновляем контент
                dayRec.Content = htmlContent;
                _appDbContext.DayRecommendations.Update(dayRec);
            }

            _appDbContext.SaveChanges();
        }

        // Удалить HTML-контент для указанной даты
        public bool RemoveHtmlByDate(DateTime date)
        {
            var dayRec = _appDbContext.DayRecommendations.FirstOrDefault(r => r.DateTime.Date == date.Date);

            if (dayRec == null) return false;

            _appDbContext.DayRecommendations.Remove(dayRec);
            _appDbContext.SaveChanges();

            return true;
        }

        // Получить все даты, для которых есть HTML-контент
        public List<DateTime> GetAllDates()
        {
            return _appDbContext.DayRecommendations
                .Select(r => r.DateTime.Date)
                .ToList();
        }
    }

}
