using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coditech.API.Data
{
    public partial class DBTMDeviceDataDetails
    {
        [Key]
        public long DBTMDeviceDataDetailId { get; set; }
        public long DBTMDeviceDataId { get; set; }

        public string ParameterCode { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal ParameterValue { get; set; }
        public string FromTo { get; set; }
        public short Row { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

