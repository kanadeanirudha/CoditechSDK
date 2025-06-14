namespace Coditech.Common.API.Model
{
    public class DBTMActivitiesDetailsModel : BaseModel
    {
        public string ParameterCode { get; set; }
        public decimal ParameterValue { get; set; }
        public string FromTo { get; set; }
        public string Row { get; set; }
    }
}
