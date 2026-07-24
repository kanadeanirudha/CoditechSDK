using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMPrintQRListModel : BaseListModel
    {
        public List<DBTMPrintQRModel> DBTMPrintQRList { get; set; }
        public DBTMPrintQRListModel()
        {
            DBTMPrintQRList = new List<DBTMPrintQRModel>();
        }
    }
}