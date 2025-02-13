using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class DBTMDeviceDataModel : BaseModel
    {
        public DBTMDeviceDataModel()
        {
            DataList = new List<DBTMDeviceDataDetailModel>();
        }
        public long DBTMDeviceDataId { get; set; }
        [MaxLength(100)]
        [Required]
        public string DeviceSerialCode { get; set; }
        [MaxLength(200)]
        [Required]
        public string PersonCode { get; set; }
        [MaxLength(50)]
        [Required]
        public string TestCode { get; set; }
        [MaxLength(200)]
        public string Comments { get; set; }
        [Required(ErrorMessage = "Please enter weight.")]
        public List<DBTMDeviceDataDetailModel> DataList { get; set; }
    }

    public class DBTMDeviceDataDetailModel
    {
        public long Time { get; set; }
        public decimal Distance { get; set; }
        public decimal Force { get; set; }
        public decimal Acceleration { get; set; }
        public decimal Angle { get; set; }
    }
}
