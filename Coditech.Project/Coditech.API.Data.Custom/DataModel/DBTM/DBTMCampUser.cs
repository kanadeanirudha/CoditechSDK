using System.ComponentModel.DataAnnotations;
namespace Coditech.API.Data
{
    public partial class DBTMCampUser
    {
        [Key]
        public long DBTMCampUserId { get; set; }
        public long DBTMCampMasterId { get; set; }
        public long DBTMTraineeDetailId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

