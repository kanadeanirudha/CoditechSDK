namespace Coditech.Common.API.Model
{
    public class DBTMMobileDashboardModel
    {
        public int NumberOfTrainees { get; set; }
        public int NumberOfBatches { get; set; }
        public int NumberOfAssignments { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DurationWithUs { get; set; }
        public string TopAthlete { get; set; }
        public List<DBTMMobileActivityCategoryModel> ActivityCategories { get; set; }
    }

    public class DBTMMobileActivityCategoryModel
    {
        public short DBTMActivityCategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}