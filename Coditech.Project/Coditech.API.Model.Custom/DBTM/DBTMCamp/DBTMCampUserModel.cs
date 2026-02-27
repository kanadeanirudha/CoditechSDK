namespace Coditech.Common.API.Model
{
    public class DBTMCampUserModel : BaseModel
    {
        public long DBTMCampUserId { get; set; }
        public int DBTMCampMasterId { get; set; }
        public long EntityId { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string ImagePath { get; set; }
        public bool IsAssociated { get; set; }
        public string BatchName { get; set; }
        public bool IsCampActive { get; set; }
    }
}
