namespace Coditech.Common.API.Model
{
    public class DBTMGeneralBatchMasterModel : BaseModel
    {
        public int DBTMGeneralBatchMasterId { get; set; }
        public int GeneralBatchMasterId { get; set; }
        public string BatchLocation { get; set; }     
    }
}
