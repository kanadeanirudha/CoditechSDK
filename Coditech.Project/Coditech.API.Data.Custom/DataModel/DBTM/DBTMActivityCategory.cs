using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMActivityCategory
    {
        [Key]
        public short DBTMActivityCategoryId { get; set; }
        public int DBTMParentActivityCategoryId { get; set; }
        public string ActivityCategoryCode { get; set; }
        public string ActivityCategoryName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }
}

