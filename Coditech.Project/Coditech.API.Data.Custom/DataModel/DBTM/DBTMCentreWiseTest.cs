using System.ComponentModel.DataAnnotations;
namespace Coditech.API.Data
{
    public partial class DBTMCentreWiseTest
    {
        [Key]
        public long DBTMCentreWiseTestId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string CentreCode { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

