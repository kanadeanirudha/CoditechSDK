using Coditech.Common.API.Model;

namespace Coditech.API.Model.Custom.DBTM.DBTMApplicationVersion
{
    public class DBTMApplicationVersionListModel : BaseListModel
    {
        public List<DBTMApplicationVersionModel> DBTMApplicationVersionList { get; set; }
        public DBTMApplicationVersionListModel()
        {
            DBTMApplicationVersionList = new List<DBTMApplicationVersionModel>();
        }
    }
}