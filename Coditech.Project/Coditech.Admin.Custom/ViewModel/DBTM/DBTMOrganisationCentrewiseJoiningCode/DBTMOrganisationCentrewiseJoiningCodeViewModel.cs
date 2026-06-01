using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMOrganisationCentrewiseJoiningCodeViewModel : BaseViewModel
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string JoiningCode { get; set; }
        public bool IsInQueue { get; set; }
        public DateTime? QueueValidTill { get; set; }
    }
}
