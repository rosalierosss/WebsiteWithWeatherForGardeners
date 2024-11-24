namespace WeatherForGardeners.DTOs
{
    public class AllChangesModel
    {
        public List<TaskCreateModel> Added { get; set; } = new();
        public List<TaskEditModel> Edited { get; set; } = new();
        public List<TaskStatusUpdateModel> UpdatedStatuses { get; set; } = new();
    }
}
