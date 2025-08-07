using System.ComponentModel.DataAnnotations;
namespace Coditech.API.Data
{
    public partial class DBTMPerformanceMatrix
    {
        [Key]
        public byte DBTMPerformanceMatrixId { get; set; }
        public string PerformanceMatrix { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

