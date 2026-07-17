using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMApplicationVersion
    {
        [Key]
        public long DBTMApplicationVersionId { get; set; }

        public string ApplicationType { get; set; }

        public string Version { get; set; }

        public string VersionDetails { get; set; }

        public string URL { get; set; }

        public bool IsLatestVersion { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}