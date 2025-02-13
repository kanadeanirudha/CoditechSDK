using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class DBTMActivitiesModel : BaseModel
    {

        public long DBTMDeviceDataId { get; set; }
        public string TestName { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(100)]
        [Required]
        public string DeviceSerialCode { get; set; }
        public string PersonCode { get; set; }
        public int NumberOfDaysRecord { get; set; }
    }
}
