using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMPerformanceMatrixViewModel : BaseViewModel
    {
        public byte DBTMPerformanceMatrixId { get; set; }
        public string PerformanceMatrix { get; set; }
        public byte? Preference { get; set; }
    }
}
