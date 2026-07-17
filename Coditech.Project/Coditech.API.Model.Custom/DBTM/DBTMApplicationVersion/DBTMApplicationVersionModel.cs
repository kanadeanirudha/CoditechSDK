using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class DBTMApplicationVersionModel : BaseModel
    {
        public long DBTMApplicationVersionId { get; set; }

        public string ApplicationType { get; set; } 

        public string Version { get; set; } 

        public string VersionDetails { get; set; } 

        public string URL { get; set; } 

        public bool IsLatestVersion { get; set; }
    }
}