namespace Coditech.Common.API.Model
{
    public class DBTMCalendarListModel : BaseListModel
    {
        public List<DBTMCalendarModel> DBTMCalendarList { get; set; }
        public DBTMCalendarListModel()
        {
            DBTMCalendarList = new List<DBTMCalendarModel>();
        }
    }
}
