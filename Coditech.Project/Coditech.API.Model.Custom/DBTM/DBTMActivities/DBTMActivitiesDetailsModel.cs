namespace Coditech.Common.API.Model
{
    public class DBTMActivitiesDetailsModel : BaseModel
    {
  
        public string TestName { get; set; }
        public long Time { get; set; }
        public decimal Distance { get; set; }
        public string TestCode { get; set; }
        public string ParameterName { get; set; }
        public byte DBTMTestParameterId { get; set; }
        public string PersonCode { get; set; }
        public decimal Force { get; set; }
        public decimal Acceleration { get; set; }
        public decimal Angle { get; set; }
        public List<string> Columns { get; set; }
    }
}
