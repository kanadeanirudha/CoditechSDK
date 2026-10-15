using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Coditech.Admin.ViewModel
{
    public class DBTMPrintQRListViewModel : BaseViewModel
    {
        public List<DBTMPrintQRViewModel> DBTMPrintQRList { get; set; }
        public DBTMPrintQRListViewModel()
        {
            DBTMPrintQRList = new List<DBTMPrintQRViewModel>();
        }
        public string SelectedCentreCode { get; set; }
        public string SelectedParameter1 { get; set; }
        public long GeneralTrainerMasterId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string PrintableHTML { get; set; }
    }
}
