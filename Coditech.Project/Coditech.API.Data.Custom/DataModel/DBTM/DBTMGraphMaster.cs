using System.ComponentModel.DataAnnotations;
namespace Coditech.API.Data
{
    public partial class DBTMGraphMaster
    {
        [Key]
        public byte DBTMGraphMasterId { get; set; }
        public string GraphName { get; set; }
        public string GraphCode { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

