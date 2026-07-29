namespace Coditech.Common.API.Model.Response
{
    public class DBTMPrintQRListResponse : BaseListResponse
    {
        public List<DBTMPrintQRModel> DBTMPrintQRList { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
