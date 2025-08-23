namespace Coditech.Common.API.Model
{
    public class DBTMCalendarModel : BaseModel
    {
        public DBTMCalendarModel()
        {
        }
        public int CalendarId { get; set; }
        public string Desc { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string Title { get; set; } = string.Empty;
        public string BackgroundColor { get; set; }
    }
}
