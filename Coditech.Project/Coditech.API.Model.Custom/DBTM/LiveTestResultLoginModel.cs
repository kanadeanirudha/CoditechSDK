namespace Coditech.Common.API.Model
{
    public class LiveTestResultLoginModel : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DeviceSerialCode { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
    }
}
