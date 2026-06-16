using System.ComponentModel.DataAnnotations;
namespace Coditech.Common.API.Model
{
    public class DBTMCentreWiseTestModel : BaseModel
    {
        public long DBTMCentreWiseTestId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public int OrganisationCentreMasterId { get; set; }
        public string CentreCode { get; set; }
        public string TestName { get; set; }
        public bool IsAssociated { get; set; }
        public List<int> TestIds { get; set; }
    }
}
