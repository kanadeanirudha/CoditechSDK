using Microsoft.EntityFrameworkCore;

namespace Coditech.API.Data
{
    public partial class CoditechCustom_Entities : CoditechDbContext
    {
        public CoditechCustom_Entities()
        {
        }

        public CoditechCustom_Entities(DbContextOptions<CoditechCustom_Entities> options) : base(options)
        {
        }
        #region DBTM
        public DbSet<DBTMDeviceMaster> DBTMDeviceMaster { get; set; }
        public DbSet<DBTMTraineeDetails> DBTMTraineeDetails { get; set; }
        public DbSet<DBTMActivityCategory> DBTMActivityCategory { get; set; }
        public DbSet<DBTMTestMaster> DBTMTestMaster { get; set; }
        public DbSet<DBTMDeviceRegistrationDetails> DBTMDeviceRegistrationDetails { get; set; }
        public DbSet<DBTMTestParameter> DBTMTestParameter { get; set; }
        public DbSet<DBTMParametersAssociatedToTest> DBTMParametersAssociatedToTest { get; set; }
        public DbSet<DBTMTraineeAssignment> DBTMTraineeAssignment { get; set; }
        public DbSet<DBTMBatchActivity> DBTMBatchActivity { get; set; }
        public DbSet<DBTMSubscriptionPlan> DBTMSubscriptionPlan { get; set; }
        public DbSet<DBTMSubscriptionPlanAssociatedToUser> DBTMSubscriptionPlanAssociatedToUser { get; set; }
        public DbSet<DBTMSubscriptionPlanActivity> DBTMSubscriptionPlanActivity { get; set; }
        public DbSet<DBTMPrivacySetting> DBTMPrivacySetting { get; set; }
        public DbSet<DBTMDeviceData> DBTMDeviceData { get; set; }
        public DbSet<DBTMDeviceDataDetails> DBTMDeviceDataDetails { get; set; }
        #endregion

    }
}
