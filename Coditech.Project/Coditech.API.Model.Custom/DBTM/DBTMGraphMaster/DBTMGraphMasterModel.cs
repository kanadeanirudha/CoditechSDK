namespace Coditech.Common.API.Model
{
    public class DBTMGraphMasterModel : BaseModel
    {
        public int DBTMGraphMasterId { get; set; }
        public string GraphName { get; set; }
        public string GraphCode { get; set; }
        public string XParameter { get; set; }
        public string YParameter { get; set; }
        public string TestCode { get; set; }
        public string GraphType { get; set; }
        public string GraphMode { get; set; }
        public List<string> DBTMSelectedTestCode { get; set; } = new List<string>();
    }
}
