namespace Coditech.Common.API.Model
{
    public class DBTMDeviceDataDetailsModel : BaseModel
    {
        public long DBTMDeviceDataDetailId { get; set; }
        public long DBTMDeviceDataId { get; set; }
        public string ParameterCode { get; set; }
        public string ParameterValue { get; set; }
        public string FromTo { get; set; }
        public short Row { get; set; }
        public string Unit { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }
        public bool IsEncrypted { get; set; }
        public List<DBTMDeviceDataDetailsModel> DBTMDeviceDataDetailsList { get; set; }
    }
}
