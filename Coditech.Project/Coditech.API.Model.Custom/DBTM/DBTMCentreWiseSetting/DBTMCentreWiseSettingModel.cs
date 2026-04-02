namespace Coditech.Common.API.Model
{
    public class DBTMCentreWiseSettingModel : BaseModel
    {
        public long DBTMCentreWiseSettingId { get; set; }
        public string CentreCode { get; set; }
        public string TypeOfCentre { get; set; }
        public int AllowBatchUser { get; set; }
        public int AllowCampUser { get; set; }
        public int OrganisationCentreMasterId { get; set; }
        public DBTMCentreWiseTestListModel TestListModel { get; set; }
    }
}
