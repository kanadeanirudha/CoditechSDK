namespace Coditech.Common.API.Model
{
    public class DBTMPrintQRModel : BaseModel
    {
        public long PersonId { get; set; }
        public int GeneralBatchMasterId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PersonCode { get; set; }
        public string QRCode { get; set; }
        public string PrintableHTML { get; set; }
    }
}
