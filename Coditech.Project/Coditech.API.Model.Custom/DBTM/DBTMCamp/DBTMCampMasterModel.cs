namespace Coditech.Common.API.Model
{
    public class DBTMCampMasterModel : BaseModel
    {
        public long DBTMCampMasterId { get; set; }    
        public string CentreCode { get; set; }       
        public string CampName { get; set; }         
        public TimeSpan CampTime { get; set; }       
        public DateTime CampStartDate { get; set; }   
        public DateTime? CampEndDate { get; set; }
        public string CampFrequency { get; set; }
        public string WeekDays { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool IsActive { get; set; }
        public List<string> SelectedWeekDays { get; set; } = new List<string>();
        public string DurationHours { get; set; }
        public string DurationMinutes { get; set; }
    }
}
