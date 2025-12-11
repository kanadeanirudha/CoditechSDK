using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMCampListViewModel : BaseViewModel
    {
        public List<DBTMCampMasterViewModel> DBTMCampMasterList { get; set; }
        public DBTMCampListViewModel()
        {
            DBTMCampMasterList = new List<DBTMCampMasterViewModel>();
        }
        public string SelectedCentreCode { get; set; } = string.Empty;
    }
}
