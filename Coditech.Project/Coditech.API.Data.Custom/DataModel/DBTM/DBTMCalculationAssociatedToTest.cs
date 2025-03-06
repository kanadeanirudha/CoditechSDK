using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMCalculationAssociatedToTest
    {
        [Key]
        public int DBTMCalculationAssociatedToTestId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public byte DBTMTestCalculationId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

