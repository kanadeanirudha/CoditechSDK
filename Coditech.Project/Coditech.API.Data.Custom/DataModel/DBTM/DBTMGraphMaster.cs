using System.ComponentModel.DataAnnotations;
namespace Coditech.API.Data
{
    public partial class DBTMGraphMaster
    {
        [Key]
        public int DBTMGraphMasterId { get; set; }
        public string GraphName { get; set; }
        public string GraphCode { get; set; }
        public string XParameter { get; set; }
        public string YParameter { get; set; }
        public string TestCode { get; set; }
        public string GraphType { get; set; }
        public string GraphMode { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

