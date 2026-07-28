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
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string PrintableHTML { get; set; }

    }
}