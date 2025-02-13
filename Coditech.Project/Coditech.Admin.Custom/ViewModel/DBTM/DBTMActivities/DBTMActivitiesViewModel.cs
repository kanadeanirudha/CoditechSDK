using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivitiesViewModel : BaseViewModel
    {
        public long DBTMDeviceDataId { get; set; }
        public string TestName { get; set; }
        public string PersonCode { get; set; }
        public int NumberOfDaysRecord { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(100)]
        [Required]
        public string DeviceSerialCode { get; set; }
    }
}
