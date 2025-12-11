namespace Coditech.Common.API.Model
{
    public class DBTMCampMasterModel : BaseModel
    {
        public long DBTMCampMasterId { get; set; }    
        public string CentreCode { get; set; }       
        public string CampName { get; set; }         
        public TimeSpan CampTime { get; set; }       
        public DateTime CampStartDate { get; set; }   
        public DateTime? CampEndDate { get; set; }
    }
}
