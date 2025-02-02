namespace Coditech.Common.API.Model
{
    public class DBTMActivityCategoryListModel : BaseListModel
    {
        public List<DBTMActivityCategoryModel> DBTMActivityCategoryList { get; set; }
        public DBTMActivityCategoryListModel()
        {
            DBTMActivityCategoryList = new List<DBTMActivityCategoryModel>();
        }

    }
}
