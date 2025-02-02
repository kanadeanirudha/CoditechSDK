using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMBatchActivity
    {
        [Key]
        public long DBTMBatchActivityId { get; set; }
        public int GeneralBatchMasterId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

