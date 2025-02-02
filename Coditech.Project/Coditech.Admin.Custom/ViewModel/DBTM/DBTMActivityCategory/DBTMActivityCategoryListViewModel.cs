using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivityCategoryListViewModel : BaseViewModel
    {
        public List<DBTMActivityCategoryViewModel> DBTMActivityCategoryList { get; set; }
        public DBTMActivityCategoryListViewModel()
        {
            DBTMActivityCategoryList = new List<DBTMActivityCategoryViewModel>();
        }
    }
}
