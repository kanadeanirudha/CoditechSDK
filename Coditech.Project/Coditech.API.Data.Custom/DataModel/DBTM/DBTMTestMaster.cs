using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTestMaster
    {
        [Key]
        public int DBTMTestMasterId { get; set; }
        public short DBTMActivityCategoryId { get; set; }
        public string TestName { get; set; }
        public string TestCode { get; set; }
        public short? MinimunPairedDevice { get; set; }
        public short? LapDistance { get; set; }
        public bool? IsLapDistanceChange { get; set; }
        public bool? IsMultiTest { get; set; }
        public string? TestInstructions { get; set; }
        public long? TestMediaId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

