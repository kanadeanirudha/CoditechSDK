using System.ComponentModel.DataAnnotations;
namespace Coditech.API.Data
{
    public partial class DBTMCampMaster
    {
        [Key]
        public int DBTMCampMasterId { get; set; }
        public string CentreCode { get; set; }
        public string CampName { get; set; }
        public DateTime CampStartDate { get; set; }
        public DateTime CampEndDate { get; set; }
        public TimeSpan CampStartTime { get; set; }
        public string CampFrequency { get; set; }
        public string WeekDays { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

