using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMCampActivity
    {
        [Key]
        public long DBTMCampActivityId { get; set; }
        public int DBTMCampMasterId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

