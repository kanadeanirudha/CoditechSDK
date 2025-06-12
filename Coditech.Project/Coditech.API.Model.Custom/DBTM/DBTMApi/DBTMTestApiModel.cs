namespace Coditech.Common.API.Model
{
    public class DBTMTestApiModel
    {
        public long DBTMTraineeAssignmentId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public TimeSpan? AssignmentTime { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        public string TestCode { get; set; }
        public short? MinimunPairedDevice { get; set; }
        public string LapDistance { get; set; }
        public bool? IsLapDistanceChange { get; set; }
        public bool? IsMultiTest { get; set; }
        public bool IsActive { get; set; }
        public string TestInstructions { get; set; }
        public List<DBTMTraineeAssignmentToUserApiModel> DBTMTraineeAssignmentToUserApiModel { get; set; }
    }
}

